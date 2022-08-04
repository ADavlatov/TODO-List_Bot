using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TODO_List_Bot.Services;

namespace TODO_List_Bot.Commands;

public static class TaskList
{
    public static async Task<Message> SendTaskList(ITelegramBotClient bot, Message message)
    {
        var tasks = HandleUpdateService.tasks;

        if (tasks.Any( )) {
            foreach (var task in tasks) {
                SendTask(bot, message, task);
            }
        }
        else
        {
            return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                text: "Список тасков пуст");
        }
        
        return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
            text: "Конец списка");
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
            text: task.Name + task.Date + task.Time,
            replyMarkup: inlineKeyboard);
    }
    
}