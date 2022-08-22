using Microsoft.Extensions.Caching.Memory;
using TaskListBot.Interfaces;
using TaskListBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TaskListBot.Commands.ReplyKeyboards;

public class Hour : ISendReplyKeyboard
{
    public Task SendReplyKeyboard(ITelegramBotClient? bot, Message message, int year = 0, int month = 0,
        int day = 0, int hour = 0)
    {
        HandleUpdateService.Cache!.Remove("SendReply" + message.From!.Id);
        HandleUpdateService.Cache.Set("ReadyToGet" + message.From.Id, "hour");

        ReplyKeyboardMarkup replyKeyboardMarkup = GetHourReplyKeybordMarkup(day);

        bot!.SendTextMessageAsync(chatId: message.Chat.Id,
            text: "Выберите час",
            replyMarkup: replyKeyboardMarkup);
        return Task.CompletedTask;
    }

    private static ReplyKeyboardMarkup GetHourReplyKeybordMarkup(int day)
    {
        IEnumerable<KeyboardButton[]> replyKeyboard;

        if (day == DateTime.Now.Day)
        {
            replyKeyboard = Enumerable.Range(DateTime.Now.Hour, 24 - DateTime.Now.Hour + 1)
                .Select(x => new KeyboardButton(x.ToString())).Chunk(7);
        }
        else
        {
            replyKeyboard = Enumerable.Range(0, 24).Select(x => new KeyboardButton(x.ToString())).Chunk(7);
        }

        ReplyKeyboardMarkup replyKeyboardMarkup = new(replyKeyboard)
        {
            ResizeKeyboard = true
        };

        return replyKeyboardMarkup;
    }
}