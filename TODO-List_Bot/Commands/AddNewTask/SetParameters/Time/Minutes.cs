using Telegram.Bot;
using Telegram.Bot.Types;
using TODO_List_Bot.Commands.AddTaskCommands;
using TODO_List_Bot.Interfaces;
using TODO_List_Bot.Services;

namespace TODO_List_Bot.Commands.AddNewTask.SetParameters.Time;

public class Minutes : ISetNewTaskParameters
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
            AddTask.minutes = Int32.Parse(message.Text);
        }

        HandleUpdateService._cache.Remove("ReadyToGet" + message.From.Id);
        HandleUpdateService._cache.Remove("Action" + message.From.Id);
        HandleUpdateService._cache.Remove("TaskAction" + message.From.Id);
        
        AddTask.AddNewTaskToList(bot, message, db);
    }
}