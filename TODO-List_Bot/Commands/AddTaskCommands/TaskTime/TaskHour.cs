using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TODO_List_Bot.Interfaces;
using TODO_List_Bot.Services;

namespace TODO_List_Bot.Commands.AddTaskCommands.TaskTime;

public class TaskHour : ISendReplyKeyboard
{
    public async Task SendReplyKeyboard(ITelegramBotClient bot, Message message)
    {
        HandleUpdateService._cache.Remove("SendReply" + message.From.Id);
        HandleUpdateService._cache.Set("ReadyToGet" + message.From.Id, "hour");
        
        ReplyKeyboardMarkup replyKeyboardMarkup = GetHourReplyKeybordMarkup();

        bot.SendTextMessageAsync(chatId: message.Chat.Id,
            text: "Выберите час",
            replyMarkup: replyKeyboardMarkup);    
    }
    private static ReplyKeyboardMarkup GetHourReplyKeybordMarkup()
    {
        var replyKeyboard = Enumerable.Range(1, 24).Select(x => new KeyboardButton(x.ToString())).Chunk(7);

        ReplyKeyboardMarkup replyKeyboardMarkup = new(replyKeyboard)
        {
            ResizeKeyboard = true
        };

        return replyKeyboardMarkup;
    }
}