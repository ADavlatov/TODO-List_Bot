using TaskListBot.Database;
using TaskListBot.Interfaces;
using TaskListBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TaskListBot.Commands.TaskActions.EditTask.Date;

public class Day : IEditTaskParameters
{
    public void EditParameter(ITelegramBotClient? bot, Message message, TaskObject? task, ApplicationContext db)
    {
        try
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Int32.Parse(message.Text!);
        }
        catch (Exception e)
        {
            bot!.SendTextMessageAsync(chatId: message.Chat.Id,
                text: "Введите цифру");
            Console.WriteLine(e);
            return;
        }
        finally
        {
            EditDate.Day = Int32.Parse(message.Text!);
        }

        HandleUpdateService.Cache!.Remove("ReadyToGet" + message.From!.Id);

#pragma warning disable CS4014
        EditDate.SetNewDate(bot, message, task, db);
#pragma warning restore CS4014
    }
}