using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot;
using Telegram.Bot.Types;
using TODO_List_Bot.Commands.AddTaskCommands;
using TODO_List_Bot.Interfaces;
using TODO_List_Bot.Services;

namespace TODO_List_Bot.Commands.AddNewTask.SetParameters;

public class Name : ISetNewTaskParameters
{
    public void SetParameter(ITelegramBotClient bot, Message message, ApplicationContext db)
    {
        AddTask.name = message.Text; 
        
        HandleUpdateService._cache.Remove("ReadyToGet" + message.From.Id);
        HandleUpdateService._cache.Set("SendReply" + message.From.Id, "year");
    }
}