using Telegram.Bot;
using Telegram.Bot.Types;

namespace TODO_List_Bot.Interfaces;

public interface ICommand
{
    void SendMessage(ITelegramBotClient bot, Message message, TaskObject task, CallbackQuery callback = null);
}