using System.Reflection.Metadata;
using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot;
using Telegram.Bot.Types;
using TODO_List_Bot.Interfaces;
using TODO_List_Bot.Services;

namespace TODO_List_Bot.Commands.AddTaskCommands;

public class SetTaskTime
{
    public static async Task SetTime(ITelegramBotClient bot, Message message)
    {
        HandleUpdateService._cache.Remove("AddAction" + message.From.Id);
        HandleUpdateService._cache.Set("AddAction" + message.From.Id, "setTaskHour");

        HandleUpdateService._cache.Set("Day" + message.From.Id, Int32.Parse(message.Text));
        
        string cache;
        if (HandleUpdateService._cache.TryGetValue("AddAction" + message.From.Id, out cache))
        {
            IAddTaskCommand? addTaskCommand = HandleUpdateService._cache.Do(cache);
            
            addTaskCommand.Add(bot, message);
        }

    }
}