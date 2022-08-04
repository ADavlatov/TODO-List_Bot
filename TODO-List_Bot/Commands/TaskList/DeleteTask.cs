using Telegram.Bot;
using Telegram.Bot.Types;
using TODO_List_Bot.Interfaces;
using TODO_List_Bot.Services;

namespace TODO_List_Bot.Commands;

public class DeleteTask : ICommand
{
    public void SendMessage(ITelegramBotClient bot, Message message, TaskObject task, CallbackQuery callback = null)
    {
        HandleUpdateService.tasks.Remove(task);

        bot.SendTextMessageAsync(chatId: callback.Message.Chat.Id,
            text: "Таск " + task.Name + " удален");
    }
}