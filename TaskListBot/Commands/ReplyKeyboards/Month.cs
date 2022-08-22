using Microsoft.Extensions.Caching.Memory;
using TaskListBot.Interfaces;
using TaskListBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TaskListBot.Commands.ReplyKeyboards;

public class Month : ISendReplyKeyboard
{
    public async Task SendReplyKeyboard(ITelegramBotClient? bot, Message message, int year = 0, int month = 0,
        int day = 0, int hour = 0)
    {
        HandleUpdateService.Cache!.Remove("SendReply" + message.From!.Id);
        HandleUpdateService.Cache.Set("ReadyToGet" + message.From.Id, "month");

        ReplyKeyboardMarkup replyKeyboardMarkup = GetKeyboardMarkup();

        await bot!.SendTextMessageAsync(chatId: message.Chat.Id,
            text: "Выберите месяц",
            replyMarkup: replyKeyboardMarkup);
    }

    private static ReplyKeyboardMarkup GetKeyboardMarkup()
    {
        ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup(
            new[]
            {
                new KeyboardButton[] { "Декабрь", "Январь", "Февраль" },
                new KeyboardButton[] { "Март", "Апрель", "Май" },
                new KeyboardButton[] { "Июнь", "Июль", "Август" },
                new KeyboardButton[] { "Сентябрь", "Октябрь", "Ноябрь" }
            })
        {
            ResizeKeyboard = true
        };

        return replyKeyboardMarkup;
    }
}