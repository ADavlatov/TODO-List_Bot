using Microsoft.Extensions.Caching.Memory;
using TaskListBot.Database;
using TaskListBot.Interfaces;
using TaskListBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TaskListBot.Commands.TaskActions.EditTask.Time;

public class EditTime : ITaskAction
{
    public static int Hour { get; set; }
    public static int Minutes { get; set; }

    public void SendMessage(ITelegramBotClient? bot, Message message, TaskObject? task, ApplicationContext db,
        CallbackQuery? callback = null)
    {
        string cache;

        if (callback != null)
        {
#pragma warning disable CS4014
            AcceptEditing(bot, task, callback);
#pragma warning restore CS4014
        }
        else if (message.Text == "Назад")
        {
            HandleUpdateService.Cache!.Remove("Action" + message.From!.Id);
            HandleUpdateService.Cache.Remove("TaskAction" + message.From.Id);
            HandleUpdateService.Cache.Remove("SendReply" + message.From.Id);
        }
        else if (HandleUpdateService.Cache!.TryGetValue("ReadyToGet" + message.From!.Id, out cache!))
        {
            IEditTaskParameters? setTaskParameters = cache.EditParameter(cache);

            setTaskParameters!.EditParameter(bot, message, task, db);
        }
        else if (HandleUpdateService.Cache!.TryGetValue("SendReply" + message.From!.Id, out cache!))
        {
            ISendReplyKeyboard? replyKeyboard = HandleUpdateService.Cache.SendReplyKeyboard(cache);

            replyKeyboard!.SendReplyKeyboard(bot, message, task!.Date.Year, task.Date.Month, task.Date.Day, Hour);
        }
    }

    public static async Task SetNewTime(ITelegramBotClient? bot, Message message, TaskObject? task,
        ApplicationContext db)
    {
        HandleUpdateService.Cache!.Remove("ReadyToGet" + message.From!.Id);
        HandleUpdateService.Cache.Remove("Action" + message.From.Id);
        HandleUpdateService.Cache.Remove("TaskAction" + message.From.Id);

        task!.Time = new TimeOnly(Hour, Minutes);
        task.State = TaskState.GetTaskState(task.Date.Year, task.Date.Month, task.Date.Day, Hour);

        db.SaveChanges();

        await bot!.SendTextMessageAsync(chatId: message.Chat.Id,
            text: "Время изменено на: " + new TimeOnly(Hour, Minutes));
    }

    private static async Task AcceptEditing(ITelegramBotClient? bot, TaskObject? task, CallbackQuery? callbackQuery)
    {
        HandleUpdateService.Cache!.Set("Action" + callbackQuery!.From.Id, "EditTask");
        HandleUpdateService.Cache!.Set("SendReply" + callbackQuery.From.Id, "hour");
        HandleUpdateService.Cache!.Set("TaskAction" + callbackQuery.From.Id, "taskTime_" + task!.Name);

        ReplyKeyboardMarkup replyKeyboardMarkup = new(
            new[]
            {
                new KeyboardButton[] { "Изменить" },
                new KeyboardButton[] { "Назад" }
            })
        {
            ResizeKeyboard = true
        };

        await bot!.SendTextMessageAsync(chatId: callbackQuery.Message!.Chat.Id,
            text: "Вы уверены что хотите изменить время?",
            replyMarkup: replyKeyboardMarkup);
    }
}