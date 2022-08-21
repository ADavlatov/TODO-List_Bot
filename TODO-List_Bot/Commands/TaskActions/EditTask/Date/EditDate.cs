using System.Globalization;
using System.Reflection.Metadata;
using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TODO_List_Bot.Interfaces;
using TODO_List_Bot.Services;

namespace TODO_List_Bot.Commands.TaskActions.EditTask.Date;

public class EditDate : ITaskAction
{
    public static int year;
    public static int month;
    public static int day;
    public void SendMessage(ITelegramBotClient bot, Message message, TaskObject task, ApplicationContext db,
        CallbackQuery callback = null)
    {
        if (callback != null)
        {
            AcceptEditing(bot, task, callback);
        }
        else if (message.Text == "Назад")
        {
            HandleUpdateService._cache.Remove("Action" + message.From.Id);
            HandleUpdateService._cache.Remove("TaskAction" + message.From.Id);
            HandleUpdateService._cache.Remove("SendReply" + message.From.Id);
        }
        
        string cache;

        if (message != null && HandleUpdateService._cache.TryGetValue("ReadyToGet" + message.From.Id, out cache))
        {
            IEditTaskParameters? setTaskParameters = cache!.EditParameter(cache);

            setTaskParameters.EditParameter(bot, message, task, db);
        }
        if (message != null && HandleUpdateService._cache.TryGetValue("SendReply" + message.From.Id, out cache))
        {
            ISendReplyKeyboard? replyKeyboard = HandleUpdateService._cache.SendReplyKeyboard(cache);

            replyKeyboard.SendReplyKeyboard(bot, message, year, month, day);
        }
    }

    public static async Task SetNewDate(ITelegramBotClient bot, Message message, TaskObject task, ApplicationContext db)
    {
        HandleUpdateService._cache.Remove("ReadyToGet" + message.From.Id);
        HandleUpdateService._cache.Remove("Action" + message.From.Id);
        HandleUpdateService._cache.Remove("TaskAction" + message.From.Id);
        
        task.Date = new DateOnly(year, month, day, new GregorianCalendar());
        task.State = TaskState.GetTaskState(year, month, day, task.Time.Hour);

        db.SaveChanges();
        
        await bot.SendTextMessageAsync(chatId: message.Chat.Id,
            text: "Дата изменена на: " + new DateOnly(year, month, day, new GregorianCalendar()));
    }

    private static async Task AcceptEditing(ITelegramBotClient bot, TaskObject task, CallbackQuery callbackQuery)
    {
        HandleUpdateService._cache.Set("Action" + callbackQuery.From.Id, "EditTask");
        HandleUpdateService._cache.Set("SendReply" + callbackQuery.From.Id, "year");
        HandleUpdateService._cache.Set("TaskAction" + callbackQuery.From.Id, "taskDate_" + task.Name);

        ReplyKeyboardMarkup replyKeyboardMarkup = new(
            new[]
            {
                new KeyboardButton[] { "Изменить" },
                new KeyboardButton[] { "Назад" }
            })
        {
            ResizeKeyboard = true
        };
            
        await bot.SendTextMessageAsync(chatId: callbackQuery.Message.Chat.Id,
            text: "Вы уверены что хотите изменить дату?",
            replyMarkup: replyKeyboardMarkup);
    }
}