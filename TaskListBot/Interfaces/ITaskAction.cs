using TaskListBot.Database;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TaskListBot.Interfaces;

public interface ITaskAction
{
    void SendMessage(ITelegramBotClient? bot, Message message, TaskObject? task, ApplicationContext db, CallbackQuery? callback = null);
}