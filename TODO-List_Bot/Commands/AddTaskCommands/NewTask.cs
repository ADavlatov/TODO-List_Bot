using System.Globalization;
using Telegram.Bot;
using Telegram.Bot.Types;
using TODO_List_Bot.Services;
using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot.Types.ReplyMarkups;
using TODO_List_Bot.Interfaces;

namespace TODO_List_Bot.Commands.AddTaskCommands;

public class NewTask : ICommand
{
    private static string name;
    private static int month;
    private static int day;
    private static int hour;
    private static int minutes;

    public static async Task<Message> CreateNewTask(ITelegramBotClient bot, Message message)
    {
        HandleUpdateService._cache.Set("TaskAction" + message.From.Id, "addTask_");
        HandleUpdateService._cache.Set("ReadyToGet" + message.From.Id, "name");
        HandleUpdateService._cache.Set("Action" + message.From.Id, "AddTask");
        
        return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
            text: "Введите название нового таск",
            replyMarkup: new ReplyKeyboardRemove());
    }

    private static async Task AddNewTask(string name, int month, int day, int hour, int minutes)
    {
        HandleUpdateService.tasks.Add(new TaskObject(name, new DateOnly(DateTime.Now.Year, month, day, new GregorianCalendar()), new TimeOnly(hour, minutes)));
    }

    public void SendMessage(ITelegramBotClient bot, Message message, TaskObject task, CallbackQuery callback = null)
    {
        SetParameters(bot, message);
    }

    private static void SetParameters(ITelegramBotClient bot, Message message)
    {
        Dictionary<string, int> months = new Dictionary<string, int>()
        {
            {"Январь", 1},
            {"Февраль", 2},
            {"Март", 3},
            {"Апрель", 4},
            {"Май", 5},
            {"Июнь", 6},
            {"Июль", 7},
            {"Август", 8},
            {"Сентябрь", 9},
            {"Октябрь", 10},
            {"Ноябрь", 11},
            {"Декабрь", 12}
        };
        
        string cache;
        if (HandleUpdateService._cache.TryGetValue("ReadyToGet" + message.From.Id, out cache))
        {
            switch (cache)
            {
                case "name":
                    name = message.Text;
                    HandleUpdateService._cache.Remove("ReadyToGet" + message.From.Id);
                    HandleUpdateService._cache.Set("SendReply" + message.From.Id, "month");
                    break;
                case "month":
                    month = months[message.Text];
                    HandleUpdateService._cache.Remove("ReadyToGet" + message.From.Id);
                    HandleUpdateService._cache.Set("SendReply" + message.From.Id, "day");
                    break;
                case "day":
                    day = Int32.Parse(message.Text);
                    HandleUpdateService._cache.Remove("ReadyToGet" + message.From.Id);
                    HandleUpdateService._cache.Set("SendReply" + message.From.Id, "hour");
                    break;
                case "hour":
                    hour = Int32.Parse(message.Text);
                    HandleUpdateService._cache.Remove("ReadyToGet" + message.From.Id);
                    HandleUpdateService._cache.Set("SendReply" + message.From.Id, "minutes");
                    break;
                case "minutes":
                    minutes = Int32.Parse(message.Text);
                    HandleUpdateService._cache.Remove("ReadyToGet" + message.From.Id);
                    HandleUpdateService._cache.Remove("Action" + message.From.Id);
                    HandleUpdateService._cache.Remove("TaskAction" + message.From.Id);
                    AddNewTask(name, month, day, hour, minutes);
                    break;
            }

            Console.WriteLine(name);
            Console.WriteLine(month);
            Console.WriteLine(day);
            Console.WriteLine(hour);
            Console.WriteLine(minutes);
        }
        if (HandleUpdateService._cache.TryGetValue("SendReply" + message.From.Id, out cache))
        {
            ISendReplyKeyboard? replyKeyboard = HandleUpdateService._cache.SendReplyKeyboard(cache);
                
            replyKeyboard.SendReplyKeyboard(bot, message);
        }
    }
} 