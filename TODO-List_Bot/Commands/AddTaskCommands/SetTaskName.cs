using System.Reflection.Metadata;
using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot;
using Telegram.Bot.Types;
using TODO_List_Bot.Interfaces;
using TODO_List_Bot.Services;

namespace TODO_List_Bot.Commands.AddTaskCommands;

public class SetTaskName : IAddTaskCommand
{
    public void Add(ITelegramBotClient bot, Message message)
    {
        HandleUpdateService._cache.Remove("AddAction" + message.From.Id);
        HandleUpdateService._cache.Set("AddAction" + message.From.Id, "setTaskMonth");

        HandleUpdateService._cache.Set("Name" + message.From.Id, message.Text);
        
        bot.SendTextMessageAsync(chatId: message.Chat.Id,
            text: "Название таска: " + message.Text);

        SetTaskDate.SetDate(bot, message);
    }
}