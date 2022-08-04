using Telegram.Bot;
using Telegram.Bot.Types;

namespace TODO_List_Bot.Interfaces;

public interface IAddTaskCommand
{
    void Add(ITelegramBotClient bot, Message message);
}