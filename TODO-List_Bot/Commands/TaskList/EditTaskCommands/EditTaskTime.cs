using Telegram.Bot;
using Telegram.Bot.Types;
using TODO_List_Bot.Interfaces;

namespace TODO_List_Bot.Commands;

public class EditTaskTime : ICommand
{
    public void SendMessage(ITelegramBotClient bot, Message message, TaskObject task, CallbackQuery callback = null)
    {
        throw new NotImplementedException();
    }
}