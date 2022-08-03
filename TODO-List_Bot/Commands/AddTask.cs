using System.Globalization;
using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TODO_List_Bot.Interfaces;
using TODO_List_Bot.Services;

namespace TODO_List_Bot.Commands;

public class AddTask : ICommand
{
    private string taskName;
    private DateOnly taskDate;
    private TimeOnly taskTime;
    private static int month;
    private static int day;
    private static int hour;
    private static int minute;

    public void SendMessage(ITelegramBotClient bot, Message message, TaskObject task, CallbackQuery callback = null)
    {
        Console.WriteLine("SendMessage");

        //@TODO переделать с использованием паттерна стратегия
        string editAction;
        if (HandleUpdateService._cache.TryGetValue("EditAction" + message.From.Id, out editAction))
        {
            switch (editAction)
            {
                case "addTaskName":
                    AddTaskName(bot, message);
                    break;
                case "addTaskDate":
                    AddTaskDate(bot, message);
                    break;
                case "addTaskTime":
                    AddTaskTime(bot, message);
                    break;
            }
        }

        if (taskName != null && taskDate != null && taskTime != null)
        {
            HandleUpdateService._cache.Remove("Action" + message.From.Id);
            HandleUpdateService.tasks.Add(new TaskObject(taskName, taskDate, taskTime));
        }
    }

    public static async Task<Message> AddNewTask(ITelegramBotClient bot, Message message)
    {
        Console.WriteLine("AddNewTask");

        HandleUpdateService._cache.Set("Action" + message.From.Id, "addTask_");
        HandleUpdateService._cache.Set("EditAction" + message.From.Id, "addTaskName");

        return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
            text: "Введите название таска",
            replyMarkup: new ReplyKeyboardRemove());
    }

    private static async Task AddTaskName(ITelegramBotClient bot, Message message)
    {
        HandleUpdateService._cache.Remove("EditAction" + message.From.Id);
        HandleUpdateService._cache.Set("EditAction" + message.From.Id, "addTaskDate");

        await bot.SendTextMessageAsync(chatId: message.Chat.Id,
            text: "Название таска: " + message.Text);

        SetTaskDate.SetTaskMonth(bot, message);
    }

    private async Task AddTaskDate(ITelegramBotClient bot, Message message)
    {
        HandleUpdateService._cache.Remove("EditAction" + message.From.Id);
        HandleUpdateService._cache.Set("EditAction" + message.From.Id, "addTaskTime");

        month = Int32.Parse(message.Text);

        await bot.SendTextMessageAsync(chatId: message.Chat.Id,
            text: "Вы выбрали " + message.Text,
            replyMarkup: new ReplyKeyboardRemove());
        
        SetTaskDate.SetTaskDay(bot, message);
    }

    private async Task AddTaskTime(ITelegramBotClient bot, Message message)
    {
        Console.WriteLine("AddTaskTime");

        HandleUpdateService._cache.Remove("EditAction" + message.From.Id);

        Console.WriteLine("Время");
    }

    
}