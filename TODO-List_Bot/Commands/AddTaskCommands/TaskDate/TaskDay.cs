using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TODO_List_Bot.Interfaces;
using TODO_List_Bot.Services;

namespace TODO_List_Bot.Commands.AddTaskCommands.TaskTime;

public class TaskDay : ISendReplyKeyboard
{
    public async Task SendReplyKeyboard(ITelegramBotClient bot, Message message)
    {
        HandleUpdateService._cache.Remove("SendReply" + message.From.Id);
        HandleUpdateService._cache.Set("ReadyToGet" + message.From.Id, "day");
        
        ReplyKeyboardMarkup replyKeyboardMarkup = GetDayReplyKeybordMarkup();

        bot.SendTextMessageAsync(chatId: message.Chat.Id,
            text: "Выберите день",
            replyMarkup: replyKeyboardMarkup); 
    }
    
    private static ReplyKeyboardMarkup GetDayReplyKeybordMarkup()
    {
        int currentDay = DateTime.Now.Day;
        int lastDay = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
        
        var replyKeyboard = Enumerable.Range(currentDay, lastDay - currentDay).Select(x => new KeyboardButton(x.ToString())).Chunk(7);

        ReplyKeyboardMarkup replyKeyboardMarkup = new(replyKeyboard)
        {
            ResizeKeyboard = true
        };

        return replyKeyboardMarkup;
    }
}