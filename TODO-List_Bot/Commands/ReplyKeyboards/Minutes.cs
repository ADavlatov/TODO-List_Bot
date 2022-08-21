using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TODO_List_Bot.Commands.AddTaskCommands;
using TODO_List_Bot.Interfaces;
using TODO_List_Bot.Services;

namespace TODO_List_Bot.Commands.ReplyKeyboards;

public class Minutes : ISendReplyKeyboard
{
    public async Task SendReplyKeyboard(ITelegramBotClient bot, Message message, int year = 0, int month = 0,
        int day = 0, int hour = 0, int minutes = 0)
    {
        HandleUpdateService._cache.Remove("SendReply" + message.From.Id);
        HandleUpdateService._cache.Set("ReadyToGet" + message.From.Id, "minutes");

        ReplyKeyboardMarkup replyKeyboardMarkup = GetMinutesReplyKeybordMarkup(year, month, day, hour);

        if (replyKeyboardMarkup == null)
        {
            bot.SendTextMessageAsync(chatId: message.From.Id,
                text: "Уведомление установлено ровно на 24:00");
        }

        bot.SendTextMessageAsync(chatId: message.Chat.Id,
            text: "Выберите минуту",
            replyMarkup: replyKeyboardMarkup);
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