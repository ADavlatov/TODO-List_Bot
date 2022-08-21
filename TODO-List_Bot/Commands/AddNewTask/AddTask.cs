using System.Globalization;
using Telegram.Bot;
using Telegram.Bot.Types;
using TODO_List_Bot.Services;
using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot.Types.ReplyMarkups;
using TODO_List_Bot.Commands.AddNewTask.SetParameters;
using TODO_List_Bot.Interfaces;

namespace TODO_List_Bot.Commands.AddTaskCommands;

public class AddTask : ITaskAction
{
    public static string name;
    public static int year;
    public static int month;
    public static int day;
    public static int hour;
    public static int minutes;

    public static async Task<Message> CreateNewTask(ITelegramBotClient bot, Message message)
    {
        HandleUpdateService._cache.Set("TaskAction" + message.From.Id, "addTask_");
        HandleUpdateService._cache.Set("ReadyToGet" + message.From.Id, "name");
        HandleUpdateService._cache.Set("Action" + message.From.Id, "AddTask");

        return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
            text: "Введите название нового таск",
            replyMarkup: new ReplyKeyboardRemove());
    }

    public static async Task AddNewTaskToList(ITelegramBotClient bot, Message message, ApplicationContext db)
    {
        db.UserTasks.Add(new TaskObject
        {
            User = db.Users.FirstOrDefault(x => x.UserId == message.From.Id), UserId = message.From.Id,
            Name = name,
            ChatId = message.Chat.Id,
            Date = new DateOnly(year, month, day, new GregorianCalendar()),
            Time = new TimeOnly(hour, minutes), State = TaskState.GetTaskState(year, month, day, hour)
        });
        db.Users.FirstOrDefault(x => x.UserId == message.From.Id).AddedTasks += 1;
        db.SaveChanges();

        await bot.SendTextMessageAsync(chatId: message.Chat.Id,
            text: "Добавлен таск:" +
                  "\r\n" +
                  "Имя: " +
                  name +
                  "\r\n" +
                  "Дата: " +
                  new DateOnly(year,
                      month,
                      day,
                      new GregorianCalendar()) +
                  "\r\n" +
                  "Время: " +
                  new TimeOnly(hour,
                      minutes));
    }

    public void SendMessage(ITelegramBotClient bot, Message message, TaskObject task, ApplicationContext db,
        CallbackQuery callback = null)
    {
        string cache;
        if (HandleUpdateService._cache.TryGetValue("ReadyToGet" + message.From.Id, out cache))
        {
            ISetNewTaskParameters? setNewTaskParameters = cache!.SetParameter(cache);

            setNewTaskParameters.SetParameter(bot, message, db);
        }

        if (HandleUpdateService._cache.TryGetValue("SendReply" + message.From.Id, out cache))
        {
            ISendReplyKeyboard? replyKeyboard = HandleUpdateService._cache.SendReplyKeyboard(cache);

            replyKeyboard.SendReplyKeyboard(bot, message, year, month, day, hour, minutes);
        }
    }
}