using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TODO_List_Bot.Interfaces;
using TODO_List_Bot.Services;

namespace TODO_List_Bot.Commands.AddTaskCommands;

public class SetTaskHour : IAddTaskCommand
{
    public void Add(ITelegramBotClient bot, Message message)
    {
        HandleUpdateService._cache.Remove("AddAction" + message.From.Id);
        HandleUpdateService._cache.Set("AddAction" + message.From.Id, "setTaskMinute");

        ReplyKeyboardMarkup replyKeyboardMarkup = GetHourReplyKeybordMarkup();

        bot.SendTextMessageAsync(chatId: message.Chat.Id,
            text: "Выберите час",
            replyMarkup: replyKeyboardMarkup);
    }
    private static ReplyKeyboardMarkup GetHourReplyKeybordMarkup()
    {
        int currentHour = DateTime.Now.Hour;
        
        var replyKeyboard = Enumerable.Range(currentHour + 1, (24 - currentHour) + 1).Select(x => new KeyboardButton(x.ToString())).Chunk(7);

        ReplyKeyboardMarkup replyKeyboardMarkup = new(replyKeyboard)
        {
            ResizeKeyboard = true
        };

        return replyKeyboardMarkup;
    }
}