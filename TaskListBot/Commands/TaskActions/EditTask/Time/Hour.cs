using Microsoft.Extensions.Caching.Memory;
using TaskListBot.Database;
using TaskListBot.Interfaces;
using TaskListBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TaskListBot.Commands.TaskActions.EditTask.Time;

public class Hour : IEditTaskParameters
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
            EditTime.Hour = Int32.Parse(message.Text!);
        }

        HandleUpdateService.Cache!.Remove("ReadyToGet" + message.From!.Id);
        HandleUpdateService.Cache.Set("SendReply" + message.From.Id, "minutes");
    }
}