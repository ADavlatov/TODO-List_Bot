using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TODO_List_Bot.Interfaces;
using TODO_List_Bot.Services;

namespace TODO_List_Bot.Commands.AddTaskCommands;

public class SetTaskMonth : IAddTaskCommand
{
    public void Add(ITelegramBotClient bot, Message message)
    {
        HandleUpdateService._cache.Remove("AddAction" + message.From.Id);
        HandleUpdateService._cache.Set("AddAction" + message.From.Id, "setTaskDay");

        var replyKeybordMarkup = GetMonthReplyKeyboardMarkup();

        bot.SendTextMessageAsync(chatId: message.Chat.Id,
            text: "Выберите месяц",
            replyMarkup: replyKeybordMarkup);
    }

    
    
    private static ReplyKeyboardMarkup GetMonthReplyKeyboardMarkup()
    {
        ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup(
            new[]
            {
                new KeyboardButton[]{"Декабрь", "Январь", "Февраль"},
                new KeyboardButton[]{"Март", "Апрель", "Май"},
                new KeyboardButton[]{"Июнь", "Июль", "Август"},
                new KeyboardButton[]{"Сентябрь", "Октябрь", "Ноябрь"}
            })
        {
            ResizeKeyboard = true
        };
        
        return replyKeyboardMarkup;
    } 
}