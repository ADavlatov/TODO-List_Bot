using TaskListBot.Database;
using TaskListBot.Interfaces;
using TaskListBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TaskListBot.Commands.AddNewTask.SetParameters.Time;

public class Minutes : ISetNewTaskParameters
{
    public void SetParameter(ITelegramBotClient? bot, Message message, ApplicationContext db)
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
            AddTask.Minutes = Int32.Parse(message.Text!);
        }

        HandleUpdateService.Cache!.Remove("ReadyToGet" + message.From!.Id);
        HandleUpdateService.Cache.Remove("Action" + message.From.Id);
        HandleUpdateService.Cache.Remove("TaskAction" + message.From.Id);

#pragma warning disable CS4014
        AddTask.AddNewTaskToList(bot, message, db);
#pragma warning restore CS4014
    }
}