using TaskListBot.Database;
using TaskListBot.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TaskListBot.Commands.TaskActions;

public class FinishTask : ITaskAction
{
    public void SendMessage(ITelegramBotClient? bot, Message message, TaskObject? task, ApplicationContext db,
        CallbackQuery? callback = null)
    {
        db.UserTasks.Remove(task!);
        db.Users.FirstOrDefault(x => x.UserId == callback!.From.Id)!.FinishedTasks += 1;

        if (task!.Date < DateOnly.FromDateTime(DateTime.Now) || task.Time < TimeOnly.FromDateTime(DateTime.Now))
        {
            db.Users.FirstOrDefault(x => x.UserId == callback!.From.Id)!.ExpiredFinishedTasks += 1;
        }

        db.SaveChanges();

        bot!.SendTextMessageAsync(chatId: callback!.Message!.Chat.Id,
            text: "Таск " + task.Name + " выполнен");
    }
}