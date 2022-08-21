using Telegram.Bot;
using Telegram.Bot.Types;
using TODO_List_Bot.Interfaces;

namespace TODO_List_Bot.Commands.TaskActions.EditTask.Time;

public class Minutes : IEditTaskParameters
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
        }
        finally
        {
            EditTime.minutes = Int32.Parse(message.Text);
        }

        EditTime.SetNewTime(bot, message, task, db);
    }
}