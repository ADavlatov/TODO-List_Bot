using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot;
using Telegram.Bot.Types;
using TODO_List_Bot.Commands.AddTaskCommands;
using TODO_List_Bot.Interfaces;
using TODO_List_Bot.Services;

namespace TODO_List_Bot.Commands.AddNewTask.SetParameters.Date;

public class Year : ISetNewTaskParameters
{
    public void SetParameter(ITelegramBotClient bot, Message message, ApplicationContext db)
    {
        try
        {
            Int32.Parse(message.Text);
        }
        catch (Exception e)
        {
            bot.SendTextMessageAsync(chatId: message.Chat.Id,
                text: "Введите цифру");
            Console.WriteLine(e);
            return;
        }
        finally
        {
            AddTask.year = Int32.Parse(message.Text);
        }


        if (Int32.Parse(message.Text) < DateTime.Now.Year ||
            Int32.Parse(message.Text) > DateTime.Now.Year + 3)
        {
            bot.SendTextMessageAsync(chatId: message.Chat.Id,
                text: "Введите цифру больше " + DateTime.Now.Year + ", но меньше " +
                      (DateTime.Now.Year + 3));
            return;
        }

        HandleUpdateService._cache.Remove("ReadyToGet" + message.From.Id);
        HandleUpdateService._cache.Set("SendReply" + message.From.Id, "month");
    }
}