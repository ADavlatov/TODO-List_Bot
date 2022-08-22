using System.Globalization;
using Microsoft.Extensions.Caching.Memory;
using TaskListBot.Database;
using TaskListBot.Interfaces;
using TaskListBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TaskListBot.Commands.AddNewTask;

public class AddTask : ITaskAction
{
    public static string? Name { get; set; }
    public static int Year { get; set; }
    public static int Month { get; set; }
    public static int Day { get; set; }
    public static int Hour { get; set; }
    public static int Minutes { get; set; }

    public static async Task<Message> CreateNewTask(ITelegramBotClient? bot, Message message)
    {
        HandleUpdateService.Cache!.Set("TaskAction" + message.From!.Id, "addTask_");
        HandleUpdateService.Cache!.Set("ReadyToGet" + message.From.Id, "name");
        HandleUpdateService.Cache!.Set("Action" + message.From.Id, "AddTask");

        return await bot!.SendTextMessageAsync(chatId: message.Chat.Id,
            text: "Введите название нового таск",
            replyMarkup: new ReplyKeyboardRemove());
    }

    public static async Task AddNewTaskToList(ITelegramBotClient? bot, Message message, ApplicationContext db)
    {
        db.UserTasks.Add(new TaskObject
        {
            User = db.Users.FirstOrDefault(x => x.UserId == message.From!.Id)!, UserId = message.From!.Id,
            Name = Name,
            ChatId = message.Chat.Id,
            Date = new DateOnly(Year, Month, Day, new GregorianCalendar()),
            Time = new TimeOnly(Hour, Minutes), State = TaskState.GetTaskState(Year, Month, Day, Hour)
        });
        db.Users.FirstOrDefault(x => x.UserId == message.From.Id)!.AddedTasks += 1;
        await db.SaveChangesAsync();

        await bot!.SendTextMessageAsync(chatId: message.Chat.Id,
            text: "Добавлен таск:" +
                  "\r\n" +
                  "Имя: " +
                  Name +
                  "\r\n" +
                  "Дата: " +
                  new DateOnly(Year,
                      Month,
                      Day,
                      new GregorianCalendar()) +
                  "\r\n" +
                  "Время: " +
                  new TimeOnly(Hour,
                      Minutes));
    }

    public void SendMessage(ITelegramBotClient? bot, Message message, TaskObject? task, ApplicationContext db,
        CallbackQuery? callback = null)
    {
        string cache;
        if (HandleUpdateService.Cache!.TryGetValue("ReadyToGet" + message.From!.Id, out cache!))
        {
            ISetNewTaskParameters? setNewTaskParameters = cache.SetParameter(cache);

            setNewTaskParameters!.SetParameter(bot, message, db);
        }

        if (HandleUpdateService.Cache!.TryGetValue("SendReply" + message.From.Id, out cache!))
        {
            ISendReplyKeyboard? replyKeyboard = HandleUpdateService.Cache.SendReplyKeyboard(cache);

            replyKeyboard!.SendReplyKeyboard(bot, message, Year, Month, Day, Hour);
        }
    }
}