using Telegram.Bot;
using Telegram.Bot.Types;
using TODO_List_Bot.Interfaces;
using TODO_List_Bot.Services;

namespace TODO_List_Bot.Commands.TaskActions.EditTask.Date;

public class Day : IEditTaskParameters
{
    public void EditParameter(ITelegramBotClient bot, Message message, TaskObject task, ApplicationContext db)
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
            EditDate.day = Int32.Parse(message.Text);
        }
        
        HandleUpdateService._cache.Remove("ReadyToGet" + message.From.Id);
        
        EditDate.SetNewDate(bot, message, task, db);
    }
}