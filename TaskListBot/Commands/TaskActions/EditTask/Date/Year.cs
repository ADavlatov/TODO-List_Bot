using Microsoft.Extensions.Caching.Memory;
using TaskListBot.Database;
using TaskListBot.Interfaces;
using TaskListBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TaskListBot.Commands.TaskActions.EditTask.Date;

public class Year : IEditTaskParameters
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
            EditDate.Year = Int32.Parse(message.Text!);
        }


        if (Int32.Parse(message.Text!) < DateTime.Now.Year ||
            Int32.Parse(message.Text!) > DateTime.Now.Year + 3)
        {
            bot!.SendTextMessageAsync(chatId: message.Chat.Id,
                text: "Введите цифру больше " + DateTime.Now.Year + ", но меньше " +
                      (DateTime.Now.Year + 3));
            return;
        }

        HandleUpdateService.Cache!.Remove("ReadyToGet" + message.From!.Id);
        HandleUpdateService.Cache.Set("SendReply" + message.From.Id, "month");
    }
}