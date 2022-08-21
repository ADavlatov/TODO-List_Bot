using System.Globalization;
using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TODO_List_Bot.Interfaces;
using TODO_List_Bot.Services;

namespace TODO_List_Bot.Commands.TaskActions.EditTask.Time;

public class EditTime : ITaskAction
{
    public static int hour;
    public static int minutes;

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

            replyKeyboard.SendReplyKeyboard(bot, message, task.Date.Year, task.Date.Month, task.Date.Day, hour, minutes);
        }
    }

    public static async Task SetNewTime(ITelegramBotClient bot, Message message, TaskObject task, ApplicationContext db)
    {
        HandleUpdateService._cache.Remove("ReadyToGet" + message.From.Id);
        HandleUpdateService._cache.Remove("Action" + message.From.Id);
        HandleUpdateService._cache.Remove("TaskAction" + message.From.Id);

        task.Time = new TimeOnly(hour, minutes);
        task.State = TaskState.GetTaskState(task.Date.Year, task.Date.Month, task.Date.Day, hour);
        
        db.SaveChanges();
        
        await bot.SendTextMessageAsync(chatId: message.Chat.Id,
            text: "Время изменено на: " + new TimeOnly(hour, minutes));
    }

    private static async Task AcceptEditing(ITelegramBotClient bot, TaskObject task, CallbackQuery callbackQuery)
    {
        HandleUpdateService._cache.Set("Action" + callbackQuery.From.Id, "EditTask");
        HandleUpdateService._cache.Set("SendReply" + callbackQuery.From.Id, "hour");
        HandleUpdateService._cache.Set("TaskAction" + callbackQuery.From.Id, "taskTime_" + task.Name);

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
            text: "Вы уверены что хотите изменить время?",
            replyMarkup: replyKeyboardMarkup);
    }
}