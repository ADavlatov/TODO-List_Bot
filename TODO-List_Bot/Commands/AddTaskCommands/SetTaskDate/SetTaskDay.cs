using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TODO_List_Bot.Interfaces;
using TODO_List_Bot.Services;

namespace TODO_List_Bot.Commands.AddTaskCommands;

public class SetTaskDay : IAddTaskCommand
{
    public void Add(ITelegramBotClient bot, Message message)
    {
        HandleUpdateService._cache.Remove("AddAction" + message.From.Id);
        HandleUpdateService._cache.Set("AddAction" + message.From.Id, "setTaskHour");

        ReplyKeyboardMarkup replyKeyboardMarkup = GetDayReplyKeybordMarkup();

        bot.SendTextMessageAsync(chatId: message.Chat.Id,
            text: "Выберите день завершения таска",
            replyMarkup: replyKeyboardMarkup);
        
        Dictionary<string, int> months = new Dictionary<string, int>()
        {
            {"Январь", 1},
            {"Февраль", 2},
            {"Март", 3},
            {"Апрель", 4},
            {"Май", 5},
            {"Июнь", 6},
            {"Июль", 7},
            {"Август", 8},
            {"Сентябрь", 9},
            {"Октябрь", 10},
            {"Ноябрь", 11},
            {"Декабрь", 12}
        };
        
        HandleUpdateService._cache.Set("Month" + message.From.Id, months[message.Text]);

        SetTaskTime.SetTime(bot, message);
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