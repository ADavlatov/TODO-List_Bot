using TaskListBot.Database;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TaskListBot.Interfaces;

public interface ISetNewTaskParameters
{
    void SetParameter(ITelegramBotClient? bot, Message message, ApplicationContext db);
}