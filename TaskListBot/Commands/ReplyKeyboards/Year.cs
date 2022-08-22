using Microsoft.Extensions.Caching.Memory;
using TaskListBot.Interfaces;
using TaskListBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TaskListBot.Commands.ReplyKeyboards;

public class Year : ISendReplyKeyboard
{
    public Task SendReplyKeyboard(ITelegramBotClient? bot, Message message, int year = 0, int month = 0,
        int day = 0, int hour = 0)
    {
        HandleUpdateService.Cache!.Remove("SendReply" + message.From!.Id);
        HandleUpdateService.Cache.Set("ReadyToGet" + message.From.Id, "year");

        ReplyKeyboardMarkup replyKeyboardMarkup = GetYearReplyKeybordMarkup();

        bot!.SendTextMessageAsync(chatId: message.Chat.Id,
            text: "Выберите год",
            replyMarkup: replyKeyboardMarkup);
        return Task.CompletedTask;
    }

    private static ReplyKeyboardMarkup GetYearReplyKeybordMarkup()
    {
        var replyKeyboard = Enumerable.Range(DateTime.Now.Year, 4).Select(x => new KeyboardButton(x.ToString()))
            .Chunk(7);

        ReplyKeyboardMarkup replyKeyboardMarkup = new(replyKeyboard)
        {
            ResizeKeyboard = true
        };

        return replyKeyboardMarkup;
    }
}