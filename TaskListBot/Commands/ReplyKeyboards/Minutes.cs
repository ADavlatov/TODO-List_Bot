using Microsoft.Extensions.Caching.Memory;
using TaskListBot.Interfaces;
using TaskListBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TaskListBot.Commands.ReplyKeyboards;

public class Minutes : ISendReplyKeyboard
{
    public Task SendReplyKeyboard(ITelegramBotClient? bot, Message message, int year = 0, int month = 0,
        int day = 0, int hour = 0)
    {
        HandleUpdateService.Cache!.Remove("SendReply" + message.From!.Id);
        HandleUpdateService.Cache.Set("ReadyToGet" + message.From.Id, "minutes");

        ReplyKeyboardMarkup replyKeyboardMarkup = GetMinutesReplyKeybordMarkup(year, month, day, hour);

        bot!.SendTextMessageAsync(chatId: message.Chat.Id,
            text: "Выберите минуту",
            replyMarkup: replyKeyboardMarkup);
        return Task.CompletedTask;
    }

    private static ReplyKeyboardMarkup GetMinutesReplyKeybordMarkup(int year, int month, int day, int hour)
    {
        IEnumerable<KeyboardButton[]> replyKeyboard;

        if (year == DateTime.Now.Year && month == DateTime.Now.Month &&
            day == DateTime.Now.Day && hour == DateTime.Now.Hour)
        {
            replyKeyboard = Enumerable.Range(DateTime.Now.Minute + 1, 60 - (DateTime.Now.Minute + 1))
                .Select(x => new KeyboardButton(x.ToString())).Chunk(12);
        }
        else
        {
            replyKeyboard = Enumerable.Range(00, 60).Select(x => new KeyboardButton(x.ToString())).Chunk(12);
        }

        ReplyKeyboardMarkup replyKeyboardMarkup = new(replyKeyboard)
        {
            ResizeKeyboard = true
        };

        return replyKeyboardMarkup;
    }
}