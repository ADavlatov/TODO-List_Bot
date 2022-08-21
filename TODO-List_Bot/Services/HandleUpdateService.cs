using System.Diagnostics;
using System.Globalization;
using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;
using TODO_List_Bot.Commands;
using TODO_List_Bot.Commands.AddTaskCommands;
using TODO_List_Bot.Commands.TaskActions;
using TODO_List_Bot.Interfaces;

namespace TODO_List_Bot.Services;

public class HandleUpdateService
{
    public static IMemoryCache _cache;
    private static ITelegramBotClient _botClient;
    private readonly ILogger<HandleUpdateService> _logger;

    // public static List<TaskObject> tasks = new()
    //     { new TaskObject("Тестовый 1", new DateOnly(2022, 08, 02, new GregorianCalendar()), new TimeOnly(18, 0), Int64.MaxValue, 0)};

    public HandleUpdateService(ITelegramBotClient botClient, ILogger<HandleUpdateService> logger,
        IMemoryCache memoryCache)
    {
        _botClient = botClient;
        _logger = logger;
        _cache = memoryCache;
    }

    public async Task EchoAsync(Update update, ApplicationContext db)
    {
        var handler = update.Type switch
        {
            UpdateType.Message => BotOnMessageReceived(update.Message!, db),
            UpdateType.EditedMessage => BotOnMessageReceived(update.EditedMessage!, db),
            UpdateType.CallbackQuery => BotOnCallbackQueryReceived(update.CallbackQuery!, update.Message, db),
            UpdateType.InlineQuery => BotOnInlineQueryReceived(update.InlineQuery!),
            UpdateType.ChosenInlineResult => BotOnChosenInlineResultReceived(update.ChosenInlineResult!),
            _ => UnknownUpdateHandlerAsync(update)
        };

        try
        {
            await handler;
        }
#pragma warning disable CA1031
        catch (Exception exception)
#pragma warning restore CA1031
        {
            await HandleErrorAsync(exception);
        }
    }

    private async Task BotOnMessageReceived(Message message, ApplicationContext db)
    {
        _logger.LogInformation("Receive message type: {MessageType}", message.Type);
        if (message.Type != MessageType.Text)
            return;

        var action = message.Text! switch
        {
            "Добавить таск" => AddTask.CreateNewTask(_botClient, message),
            "Список тасков" => TaskList.SendTaskList(_botClient, message, db),
            "Статистика" => UserStats.ShowUserStats(_botClient, message, db),
            _ => OnMessageReceived(_botClient, message)
        };
        Message sentMessage = await action;
        _logger.LogInformation("The message was sent with id: {SentMessageId}", sentMessage.MessageId);

        async Task<Message> OnMessageReceived(ITelegramBotClient bot, Message message)
        {
            if (db.Users.FirstOrDefault(x => x.UserId == message.From.Id) == null)
            {
                db.Users.Add(new User { UserId = message.From.Id });
                db.SaveChanges();
            }

            string cache;
            if (_cache.TryGetValue("TaskAction" + message.From.Id, out cache))
            {
                var splitedCache = cache.Split("_");
                var task = db.UserTasks.FirstOrDefault(x => x.Name == splitedCache[1]);

                ITaskAction? command = cache.Do(splitedCache[0]);
                command.SendMessage(bot, message, task, db);
            }

            if (!_cache.TryGetValue("Action" + message.From.Id, out cache))
            {
                ReplyKeyboardMarkup replyKeyboardMarkup = new(
                    new[]
                    {
                        new KeyboardButton[] { "Список тасков" },
                        new KeyboardButton[] { "Добавить таск" },
                        new KeyboardButton[] { "Статистика" }
                    })
                {
                    ResizeKeyboard = true
                };

                return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                    text: "Выберите",
                    replyMarkup: replyKeyboardMarkup);
            }

            return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                text: "").ConfigureAwait(false);
        }
    }

    public static async Task SendTaskNotification(TaskObject? task)
    {
        switch (task.State)
        {
            case 3:
                _botClient.SendTextMessageAsync(chatId: task.ChatId,
                    text: "Завтра истекает время таска: " + task.Name);
                break;
            case 2:
                _botClient.SendTextMessageAsync(chatId: task.ChatId,
                    text: "Через час истекает время таска: " + task.Name);
                break;
            case 1:
                _botClient.SendTextMessageAsync(chatId: task.ChatId,
                    text: "Напоминаем об: " + task.Name);
                break;
        }

        task.State -= 1;
    }

    // Process Inline Keyboard callback data
    private async Task BotOnCallbackQueryReceived(CallbackQuery callbackQuery, Message message, ApplicationContext db)
    {
        var action = callbackQuery.Data.Split("_");
        var task = db.UserTasks.FirstOrDefault(x => x.User.UserId == callbackQuery.From.Id && x.Name == action[1]);
        
        ITaskAction? taskAction = task!.Do(action[0]);

        taskAction.SendMessage(_botClient, message, task, db, callbackQuery);
    }

    #region Inline Mode

    private async Task BotOnInlineQueryReceived(InlineQuery inlineQuery)
    {
        _logger.LogInformation("Received inline query from: {InlineQueryFromId}", inlineQuery.From.Id);

        InlineQueryResult[] results =
        {
            // displayed result
            new InlineQueryResultArticle(
                id: "3",
                title: "TgBots",
                inputMessageContent: new InputTextMessageContent(
                    "hello"
                )
            )
        };

        await _botClient.AnswerInlineQueryAsync(inlineQueryId: inlineQuery.Id,
            results: results,
            isPersonal: true,
            cacheTime: 0);
    }

    private Task BotOnChosenInlineResultReceived(ChosenInlineResult chosenInlineResult)
    {
        _logger.LogInformation("Received inline result: {ChosenInlineResultId}", chosenInlineResult.ResultId);
        return Task.CompletedTask;
    }

    #endregion

    private Task UnknownUpdateHandlerAsync(Update update)
    {
        _logger.LogInformation("Unknown update type: {UpdateType}", update.Type);
        return Task.CompletedTask;
    }

    public Task HandleErrorAsync(Exception exception)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException =>
                $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        _logger.LogInformation("HandleError: {ErrorMessage}", ErrorMessage);
        return Task.CompletedTask;
    }
}