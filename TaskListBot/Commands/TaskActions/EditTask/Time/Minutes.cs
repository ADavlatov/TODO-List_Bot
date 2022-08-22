using TaskListBot.Database;
using TaskListBot.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TaskListBot.Commands.TaskActions.EditTask.Time;

public class Minutes : IEditTaskParameters
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
        }
        finally
        {
            EditTime.Minutes = Int32.Parse(message.Text!);
        }

#pragma warning disable CS4014
        EditTime.SetNewTime(bot, message, task, db);
#pragma warning restore CS4014
    }
}