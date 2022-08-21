using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot;
using Telegram.Bot.Types;
using TODO_List_Bot.Interfaces;
using TODO_List_Bot.Services;

namespace TODO_List_Bot.Commands;

public class EditTaskName : ITaskAction
{
    public void SendMessage(ITelegramBotClient bot, Message message, TaskObject task, ApplicationContext db,
        CallbackQuery callback = null)
    {
        string action;
        if (message != null && HandleUpdateService._cache.TryGetValue("TaskAction" + message.From.Id, out action))
        {
            HandleUpdateService._cache.Remove("TaskAction" + message.From.Id);
            ChangeTaskName(bot, message, task, db);
        }
        else
        {
            bot.SendTextMessageAsync(chatId: callback.Message.Chat.Id,
                text: "Введите новое название таска"); 
        
            HandleUpdateService._cache.Set("TaskAction" + callback.From.Id, "taskName_" + task.Name);
        }
    }

    static async Task ChangeTaskName(ITelegramBotClient bot, Message message, TaskObject task, ApplicationContext db)
    {
        var user = db.Users.FirstOrDefault(x => x.UserId == message.From.Id);
        task.Name = message.Text;
        user.EditedTask += 1;

        db.SaveChanges();
        
        await bot.SendTextMessageAsync(chatId: message.Chat.Id,
            text: "Имя таска изменено на " + task.Name);
    }
}