using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot;
using Telegram.Bot.Types;
using TODO_List_Bot.Interfaces;
using TODO_List_Bot.Services;

namespace TODO_List_Bot.Commands;

public class EditTaskName : ICommand
{
    public void SendMessage(ITelegramBotClient bot, Message message, TaskObject task, CallbackQuery callback = null)
    {
        if (callback != null)
        {
            bot.SendTextMessageAsync(chatId: callback.Message.Chat.Id,
                text: "Введите новое название таска"); 
        
            HandleUpdateService._cache.Set("Action" + callback.From.Id, "taskName_" + task.Name);
        }
        
        string action;
        if (message != null && HandleUpdateService._cache.TryGetValue("Action" + message.From.Id, out action))
        {
            HandleUpdateService._cache.Remove("Action" + message.From.Id);
            ChangeTaskName(bot, message, task);
        }
    }

    static async Task ChangeTaskName(ITelegramBotClient bot, Message message, TaskObject task)
    {
        task.Name = message.Text;
        
        await bot.SendTextMessageAsync(chatId: message.Chat.Id,
            text: "Имя таска изменено на " + task.Name);
    }
}