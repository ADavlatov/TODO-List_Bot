using Microsoft.Extensions.Caching.Memory;
using TaskListBot.Database;
using TaskListBot.Interfaces;
using TaskListBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TaskListBot.Commands.TaskActions.EditTask;

public class Name : ITaskAction
{
    public void SendMessage(ITelegramBotClient? bot, Message message, TaskObject? task, ApplicationContext db,
        CallbackQuery? callback = null)
    {
        // ReSharper disable once NotAccessedVariable
        string action;
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (message != null && HandleUpdateService.Cache!.TryGetValue("TaskAction" + message.From!.Id, out action!))
        {
            HandleUpdateService.Cache!.Remove("TaskAction" + message.From.Id);
#pragma warning disable CS4014
            ChangeTaskName(bot, message, task, db);
#pragma warning restore CS4014
        }
        else
        {
            bot!.SendTextMessageAsync(chatId: callback!.Message!.Chat.Id,
                text: "Введите новое название таска");

            HandleUpdateService.Cache!.Set("TaskAction" + callback.From.Id, "taskName_" + task!.Name);
        }
    }

    static async Task ChangeTaskName(ITelegramBotClient? bot, Message message, TaskObject? task, ApplicationContext db)
    {
        var user = db.Users.FirstOrDefault(x => x.UserId == message.From!.Id);
        task!.Name = message.Text;
        user!.EditedTask += 1;

        db.SaveChanges();

        await bot!.SendTextMessageAsync(chatId: message.Chat.Id,
            text: "Имя таска изменено на " + task.Name);
    }
}