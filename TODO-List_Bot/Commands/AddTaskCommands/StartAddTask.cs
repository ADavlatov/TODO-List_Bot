using System.Reflection.Metadata;
using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TODO_List_Bot.Services;

namespace TODO_List_Bot.Commands.AddTaskCommands;

public class StartAddTask
{
    public static async Task<Message> AddNewTask(ITelegramBotClient bot, Message message)
    {
        HandleUpdateService._cache.Set("Action" + message.From.Id, "addTask");
        HandleUpdateService._cache.Set("AddAction" + message.From.Id, "setTaskName");

        return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
            text: "Введите название таска",
            replyMarkup: new ReplyKeyboardRemove());
    }
}