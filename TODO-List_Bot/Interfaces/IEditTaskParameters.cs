using Telegram.Bot;
using Telegram.Bot.Types;

namespace TODO_List_Bot.Interfaces;

public interface IEditTaskParameters
{
    void EditParameter(ITelegramBotClient bot, Message message, TaskObject task, ApplicationContext db);
}