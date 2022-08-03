using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TODO_List_Bot.Commands;

public class SetTaskDate
{
    public static async Task SetTaskMonth(ITelegramBotClient bot, Message message)
    {
        ReplyKeyboardMarkup replyKeyboardMarkup = GetSelectMonthReplyKeybordMarkup();

        await bot.SendTextMessageAsync(chatId: message.Chat.Id,
            text: "Выберите месяц завершения таска",
            replyMarkup: replyKeyboardMarkup);
    }

    public static async Task SetTaskDay(ITelegramBotClient bot, Message message)
    {
        ReplyKeyboardMarkup replyKeyboardMarkup = GetSelectDayReplyKeybordMarkup();
        
        await bot.SendTextMessageAsync(chatId: message.Chat.Id,
            text: "Выберите день завершения таска",
            replyMarkup: replyKeyboardMarkup);
    }

    private static ReplyKeyboardMarkup GetSelectMonthReplyKeybordMarkup()
    {
        ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup(
            new[]
            {
                new KeyboardButton[]{"Декабрь", "Январь", "Февраль"},
                new KeyboardButton[]{"Март", "Апрель", "Май"},
                new KeyboardButton[]{"Июнь", "Июль", "Август"},
                new KeyboardButton[]{"Сентябрь", "Октябрь", "Ноябрь"}
            })
        {
            ResizeKeyboard = true
        };
        
        return replyKeyboardMarkup;
    } 
    private static ReplyKeyboardMarkup GetSelectDayReplyKeybordMarkup()
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