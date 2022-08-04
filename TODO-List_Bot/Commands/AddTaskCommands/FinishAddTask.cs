using System.Globalization;
using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot;
using Telegram.Bot.Types;
using TODO_List_Bot.Services;

namespace TODO_List_Bot.Commands.AddTaskCommands;

public class FinishAddTask
{
    public static async Task AddTask(ITelegramBotClient bot, Message message)
    {
        HandleUpdateService._cache.Remove("Action" + message.From.Id);

        string name = HandleUpdateService._cache.Get("Name" + message.From.Id).ToString();
        int month = (int)HandleUpdateService._cache.Get("Month" + message.From.Id);
        int day = (int)HandleUpdateService._cache.Get("Day" + message.From.Id);
        int hour = (int)HandleUpdateService._cache.Get("Hour" + message.From.Id);
        var minute = Int32.Parse(message.Text);
        
        Console.WriteLine(name, month, day, hour, minute);
        
        HandleUpdateService.tasks.Add(new TaskObject(name, new DateOnly(DateTime.Now.Year, month, day, new GregorianCalendar()), new TimeOnly(hour, minute)));
    }
}