using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot;
using Telegram.Bot.Types;
using TODO_List_Bot.Commands.AddTaskCommands;
using TODO_List_Bot.Interfaces;
using TODO_List_Bot.Services;

namespace TODO_List_Bot.Commands.AddNewTask.SetParameters.Date;

public class Month : ISetNewTaskParameters
{
    Dictionary<string, int> months = new()
    {
        { "Январь", 1 },
        { "Февраль", 2 },
        { "Март", 3 },
        { "Апрель", 4 },
        { "Май", 5 },
        { "Июнь", 6 },
        { "Июль", 7 },
        { "Август", 8 },
        { "Сентябрь", 9 },
        { "Октябрь", 10 },
        { "Ноябрь", 11 },
        { "Декабрь", 12 }
    };
    
    public void SetParameter(ITelegramBotClient bot, Message message, ApplicationContext db)
    {
        try
        {
            AddTask.month = months[message.Text];
        }
        catch (Exception e)
        {
            bot.SendTextMessageAsync(chatId: message.Chat.Id,
                text: "Введите корректное название месяца или выберите нужный на клавиатуре");
            Console.WriteLine(e);
            return;
        }
        finally
        {
            AddTask.month = months[message.Text];
        }

        if (DateTime.Now.Year == AddTask.year && AddTask.month < DateTime.Now.Month)
        {
            bot.SendTextMessageAsync(chatId: message.From.Id,
                text: "Нельзя поставить уведомления в прошлое");
            return;
        }

        HandleUpdateService._cache.Remove("ReadyToGet" + message.From.Id);
        HandleUpdateService._cache.Set("SendReply" + message.From.Id, "day");
    }
}