using Microsoft.Extensions.Caching.Memory;
using TaskListBot.Interfaces;
using TaskListBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TaskListBot.Commands.ReplyKeyboards;

public class Day : ISendReplyKeyboard
{
    public Task SendReplyKeyboard(ITelegramBotClient? bot, Message message, int year = 0, int month = 0,
        int day = 0, int hour = 0)
    {
        HandleUpdateService.Cache!.Remove("SendReply" + message.From!.Id);
        HandleUpdateService.Cache.Set("ReadyToGet" + message.From.Id, "day");

        ReplyKeyboardMarkup replyKeyboardMarkup = GetDayReplyKeybordMarkup(year, month);

        bot!.SendTextMessageAsync(chatId: message.Chat.Id,
            text: "Выберите день",
            replyMarkup: replyKeyboardMarkup);
        return Task.CompletedTask;
    }

    private static ReplyKeyboardMarkup GetDayReplyKeybordMarkup(int year, int month)
    {
        int currentDay = DateTime.Now.Day;
        int lastDay = DateTime.DaysInMonth(year, month);
        IEnumerable<KeyboardButton[]> replyKeyboard;

        if (month == DateTime.Now.Month)
        {
            replyKeyboard = Enumerable.Range(currentDay, lastDay - currentDay)
                .Select(x => new KeyboardButton(x.ToString())).Chunk(7);
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