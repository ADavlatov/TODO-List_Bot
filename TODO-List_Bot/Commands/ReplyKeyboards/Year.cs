using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TODO_List_Bot.Interfaces;
using TODO_List_Bot.Services;

namespace TODO_List_Bot.Commands.ReplyKeyboards;

public class Year : ISendReplyKeyboard
{
    public async Task SendReplyKeyboard(ITelegramBotClient bot, Message message, int year = 0, int month = 0,
        int day = 0, int hour = 0, int minutes = 0)
    {
        HandleUpdateService._cache.Remove("SendReply" + message.From.Id);
        HandleUpdateService._cache.Set("ReadyToGet" + message.From.Id, "year");
        
        ReplyKeyboardMarkup replyKeyboardMarkup = GetYearReplyKeybordMarkup();
        
        bot.SendTextMessageAsync(chatId: message.Chat.Id,
            text: "Выберите год",
            replyMarkup: replyKeyboardMarkup); 
    }
    private static ReplyKeyboardMarkup GetYearReplyKeybordMarkup()
    {
        var replyKeyboard = Enumerable.Range(DateTime.Now.Year, 4).Select(x => new KeyboardButton(x.ToString())).Chunk(7);
        
        ReplyKeyboardMarkup replyKeyboardMarkup = new(replyKeyboard)
        {
            ResizeKeyboard = true
        };

        return replyKeyboardMarkup;
    }
}