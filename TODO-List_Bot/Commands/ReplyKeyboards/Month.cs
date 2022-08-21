using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TODO_List_Bot.Interfaces;
using TODO_List_Bot.Services;

namespace TODO_List_Bot.Commands.ReplyKeyboards;

public class Month : ISendReplyKeyboard
{
    public async Task SendReplyKeyboard(ITelegramBotClient bot, Message message, int year = 0, int month = 0,
        int day = 0, int hour = 0, int minutes = 0)
    {
        HandleUpdateService._cache.Remove("SendReply" + message.From.Id);
        HandleUpdateService._cache.Set("ReadyToGet" + message.From.Id, "month");

        ReplyKeyboardMarkup replyKeyboardMarkup = GetKeyboardMarkup();
        
        await bot.SendTextMessageAsync(chatId: message.Chat.Id,
            text: "Выберите месяц",
            replyMarkup: replyKeyboardMarkup);
    }
    
    private static ReplyKeyboardMarkup GetKeyboardMarkup()
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