using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TODO_List_Bot.Commands.TaskActions;

public static class TaskList
{
    public static async Task<Message> SendTaskList(ITelegramBotClient bot, Message message, ApplicationContext db)
    {
        if (db.UserTasks.FirstOrDefault(x => x.User.UserId == message.From.Id) != null)
        {
            foreach (var task in db.UserTasks)
            {
                if (task.User.UserId == message.From.Id)
                {
                    SendTask(bot, message, task);
                }
            }
        }
        else
        {
            return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                text: "Список тасков пуст");
        }

        return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
            text: "").ConfigureAwait(false);
    }

    private static void SendTask(ITelegramBotClient bot, Message message, TaskObject task)
    {
        InlineKeyboardMarkup inlineKeyboard = new(
            new[]
            {
                InlineKeyboardButton.WithCallbackData("✅", "finish_" + task.Name),
                InlineKeyboardButton.WithCallbackData("🖋", "edit_" + task.Name),
                InlineKeyboardButton.WithCallbackData("🚫", "delete_" + task.Name)
            });

        bot.SendTextMessageAsync(chatId: message.Chat.Id,
            text: task.Name + "\r\n" + "Выполнить до: " + task.Date + "\r\n" + "Уведомление придет в: " + task.Time,
            replyMarkup: inlineKeyboard);
    }
}