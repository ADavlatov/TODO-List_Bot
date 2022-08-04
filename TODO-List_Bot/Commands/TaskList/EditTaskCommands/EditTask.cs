using System.Reflection.Metadata;
using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TODO_List_Bot.Interfaces;
using TODO_List_Bot.Services;

namespace TODO_List_Bot.Commands;

public class EditTask : ICommand
{
    public void SendMessage(ITelegramBotClient bot, Message message, TaskObject task, CallbackQuery callback = null)
    {
        SendEditMenu(bot, callback.Message, task);
    }

    private async Task SendEditMenu(ITelegramBotClient bot, Message message, TaskObject task)
    {
        InlineKeyboardMarkup inlineKeyboardMarkup = new(
            new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Изменить название", "taskName_" + task.Name)
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

        await bot.SendTextMessageAsync(chatId: message.Chat.Id,
            text: "Выберите что хотите изменить",
            replyMarkup: inlineKeyboardMarkup);
    }
}