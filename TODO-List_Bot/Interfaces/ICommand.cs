using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TODO_List_Bot.Interfaces;

public interface ICommand
{
    void SendMessage(ITelegramBotClient bot, Message message, TaskObject task);
}