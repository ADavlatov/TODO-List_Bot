using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TODO_List_Bot.Commands.AddTaskCommands;
using TODO_List_Bot.Interfaces;
using TODO_List_Bot.Services;

namespace TODO_List_Bot.Commands.ReplyKeyboards;

public class Day : ISendReplyKeyboard
{
    public async Task SendReplyKeyboard(ITelegramBotClient bot, Message message, int year = 0, int month = 0,
        int day = 0, int hour = 0, int minutes = 0)
    {
        HandleUpdateService._cache.Remove("SendReply" + message.From.Id);
        HandleUpdateService._cache.Set("ReadyToGet" + message.From.Id, "day");
        
        ReplyKeyboardMarkup replyKeyboardMarkup = GetDayReplyKeybordMarkup(year, month);
        
        bot.SendTextMessageAsync(chatId: message.Chat.Id,
            text: "Выберите день",
            replyMarkup: replyKeyboardMarkup); 
    }
    
    private static ReplyKeyboardMarkup GetDayReplyKeybordMarkup(int year, int month)
    {
        int currentDay = DateTime.Now.Day;
        int lastDay = DateTime.DaysInMonth(year, month);
        IEnumerable<KeyboardButton[]> replyKeyboard;
        
        if (month == DateTime.Now.Month)
        {
            replyKeyboard = Enumerable.Range(currentDay, lastDay - currentDay).Select(x => new KeyboardButton(x.ToString())).Chunk(7);
        }
        else
        {
            replyKeyboard = Enumerable.Range(1, lastDay).Select(x => new KeyboardButton(x.ToString())).Chunk(7);
        }
        
        ReplyKeyboardMarkup replyKeyboardMarkup = new(replyKeyboard)
        {
            ResizeKeyboard = true
        };

        return replyKeyboardMarkup;
    }
}