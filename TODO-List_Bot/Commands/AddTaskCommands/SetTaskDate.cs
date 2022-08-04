using Telegram.Bot;
using Telegram.Bot.Types;
using TODO_List_Bot.Interfaces;
using TODO_List_Bot.Services;

namespace TODO_List_Bot.Commands.AddTaskCommands;

public class SetTaskDate
{
    public static async Task SetDate(ITelegramBotClient bot, Message message)
    {
        object? cache;
        if (HandleUpdateService._cache.TryGetValue("AddAction" + message.From.Id, out cache))
        {
            IAddTaskCommand? addTaskCommand = HandleUpdateService._cache.Do(cache.ToString());
            
            addTaskCommand.Add(bot, message);
        }
    }

    public static DateOnly GetDate()
    {
        return getDate
    }
}