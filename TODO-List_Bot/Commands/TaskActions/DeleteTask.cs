using Telegram.Bot;
using Telegram.Bot.Types;
using TODO_List_Bot.Interfaces;
using TODO_List_Bot.Services;

namespace TODO_List_Bot.Commands;

public class DeleteTask : ITaskAction
{
    public void SendMessage(ITelegramBotClient bot, Message message, TaskObject task, ApplicationContext db,
        CallbackQuery callback = null)
    {
        db.UserTasks.Remove(task);
        db.Users.FirstOrDefault(x => x.UserId == callback.From.Id).DeletedTasks += 1;
        
        if (task.Date < DateOnly.FromDateTime(DateTime.Now) || task.Time < TimeOnly.FromDateTime(DateTime.Now))
        {
            db.Users.FirstOrDefault(x => x.UserId == callback.From.Id).ExpiredDeletedTasks += 1;
        }
        
        db.SaveChanges();

        bot.SendTextMessageAsync(chatId: callback.Message.Chat.Id,
            text: "Таск " + task.Name + " удален");
    }
}