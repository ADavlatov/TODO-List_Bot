using Telegram.Bot;
using Telegram.Bot.Types;

namespace TODO_List_Bot.Interfaces;

public interface ITaskAction
{
    void SendMessage(ITelegramBotClient bot, Message message, TaskObject task, ApplicationContext db, CallbackQuery callback = null);
}