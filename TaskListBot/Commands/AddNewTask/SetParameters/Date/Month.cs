using Microsoft.Extensions.Caching.Memory;
using TaskListBot.Database;
using TaskListBot.Interfaces;
using TaskListBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TaskListBot.Commands.AddNewTask.SetParameters.Date;

public class Month : ISetNewTaskParameters
{
    private readonly Dictionary<string, int> _months = new()
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

    public void SetParameter(ITelegramBotClient? bot, Message message, ApplicationContext db)
    {
        try
        {
            AddTask.Month = _months[message.Text!];
        }
        catch (Exception e)
        {
            bot!.SendTextMessageAsync(chatId: message.Chat.Id,
                text: "Введите корректное название месяца или выберите нужный на клавиатуре");
            Console.WriteLine(e);
            return;
        }
        finally
        {
            AddTask.Month = _months[message.Text!];
        }

        if (DateTime.Now.Year == AddTask.Year && AddTask.Month < DateTime.Now.Month)
        {
            bot!.SendTextMessageAsync(chatId: message.From!.Id,
                text: "Нельзя поставить уведомления в прошлое");
            return;
        }

        HandleUpdateService.Cache!.Remove("ReadyToGet" + message.From!.Id);
        HandleUpdateService.Cache.Set("SendReply" + message.From.Id, "day");
    }
}