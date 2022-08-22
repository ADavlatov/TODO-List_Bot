using Microsoft.Extensions.Caching.Memory;
using TaskListBot.Database;
using TaskListBot.Interfaces;
using TaskListBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TaskListBot.Commands.AddNewTask.SetParameters;

public class Name : ISetNewTaskParameters
{
    public void SetParameter(ITelegramBotClient? bot, Message message, ApplicationContext db)
    {
        AddTask.Name = message.Text!;

        HandleUpdateService.Cache!.Remove("ReadyToGet" + message.From!.Id);
        HandleUpdateService.Cache.Set("SendReply" + message.From.Id, "year");
    }
}