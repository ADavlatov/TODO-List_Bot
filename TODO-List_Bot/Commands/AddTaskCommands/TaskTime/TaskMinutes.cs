using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TODO_List_Bot.Interfaces;
using TODO_List_Bot.Services;

namespace TODO_List_Bot.Commands.AddTaskCommands.TaskTime;

public class TaskMinutes : ISendReplyKeyboard
{
    public async Task SendReplyKeyboard(ITelegramBotClient bot, Message message)
    {
        HandleUpdateService._cache.Remove("SendReply" + message.From.Id);
        HandleUpdateService._cache.Set("ReadyToGet" + message.From.Id, "minutes");
        
        ReplyKeyboardMarkup replyKeyboardMarkup = GetHourReplyKeybordMarkup();

        bot.SendTextMessageAsync(chatId: message.Chat.Id,
            text: "Выберите минуту",
            replyMarkup: replyKeyboardMarkup);
    }
    private static ReplyKeyboardMarkup GetHourReplyKeybordMarkup()
    {
        var replyKeyboard = Enumerable.Range(00, 60).Select(x => new KeyboardButton(x.ToString())).Chunk(12);

        ReplyKeyboardMarkup replyKeyboardMarkup = new(replyKeyboard)
        {
            ResizeKeyboard = true
        };

        return replyKeyboardMarkup;
    }
}