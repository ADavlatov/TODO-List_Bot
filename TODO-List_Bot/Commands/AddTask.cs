using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot;
using Telegram.Bot.Types;
using TODO_List_Bot.Interfaces;
using TODO_List_Bot.Services;

namespace TODO_List_Bot.Commands;

public class AddTask : ICommand
{
    public void SendMessage(ITelegramBotClient bot, Message message, TaskObject task = null, CallbackQuery callback = null)
    {
        AddTaskName(bot, message, task);
    }

    public static async Task<Message> AddNewTask(ITelegramBotClient bot, Message message)
    {
        HandleUpdateService._cache.Set("Action" + message.From.Id, "addTask_");
        
        return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
            text: "Введите название таска"); 
    }
    private async Task AddTaskName(ITelegramBotClient bot, Message message, TaskObject task)
    {
        HandleUpdateService._cache.Remove("Action" + message.From.Id);
        HandleUpdateService.tasks.Add(new TaskObject(message.Text));
        
        await bot.SendTextMessageAsync(chatId: message.Chat.Id,
            text: "Название таска: " + message.Text);
        
    }
    //@TODO добавить поддержку времени
    // private async Task AddTaskDate(ITelegramBotClient bot, Message message, TaskObject task)
    // {
    //     
    // }
    // private async Task AddTaskTime(ITelegramBotClient bot, Message message, TaskObject task)
    // {
    //     
    // }
    
}