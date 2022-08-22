using Microsoft.Extensions.Caching.Memory;
using TaskListBot.Database;
using TaskListBot.Interfaces;
using TaskListBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TaskListBot.Commands.AddNewTask.SetParameters.Date;

public class Day : ISetNewTaskParameters
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
            AddTask.Day = Int32.Parse(message.Text!);
        }

        HandleUpdateService.Cache!.Remove("ReadyToGet" + message.From!.Id);
        HandleUpdateService.Cache.Set("SendReply" + message.From.Id, "hour");
    }
}