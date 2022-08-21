using Telegram.Bot;
using Telegram.Bot.Types;

namespace TODO_List_Bot.Interfaces;

public interface ISetNewTaskParameters
{
    void SetParameter(ITelegramBotClient bot, Message message, ApplicationContext db);
}