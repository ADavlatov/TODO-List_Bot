using TaskListBot.Database;
using TaskListBot.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TaskListBot.Commands.TaskActions.EditTask;

public class EditMenu : ITaskAction
{
    public void SendMessage(ITelegramBotClient? bot, Message message, TaskObject? task, ApplicationContext db,
        CallbackQuery? callback = null)
    {
#pragma warning disable CS4014
        SendEditMenu(bot, callback!.Message!, task);
#pragma warning restore CS4014
    }

    private async Task SendEditMenu(ITelegramBotClient? bot, Message message, TaskObject? task)
    {
        InlineKeyboardMarkup inlineKeyboardMarkup = new(
            new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Изменить название", "taskName_" + task!.Name)
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Изменить дату", "taskDate_" + task.Name)
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Изменить время", "taskTime_" + task.Name)
                }
            }
        );

        await bot!.SendTextMessageAsync(chatId: message.Chat.Id,
            text: "Выберите что хотите изменить",
            replyMarkup: inlineKeyboardMarkup);
    }
}