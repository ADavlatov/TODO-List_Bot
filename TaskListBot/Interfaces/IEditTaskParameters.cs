using TaskListBot.Database;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TaskListBot.Interfaces;

public interface IEditTaskParameters
{
    void EditParameter(ITelegramBotClient? bot, Message message, TaskObject? task, ApplicationContext db);
}