using Telegram.Bot;
using Telegram.Bot.Types;

namespace TODO_List_Bot.Commands;

public class UserStats
{
    public static async Task<Message> ShowUserStats(ITelegramBotClient bot, Message message, ApplicationContext db)
    {
        var user = db.Users.FirstOrDefault(x => x.UserId == message.From.Id);

        return await bot.SendTextMessageAsync(chatId: message.From.Id,
            text: "Добавлено тасков: " + user.AddedTasks +
                  "\r\n" +
                  "Завершено тасков: " + user.FinishedTasks +
                  "\r\n" +
                  "Удалено тасков: " + user.DeletedTasks +
                  "\r\n" +
                  "Завершено после истечения времени: " + user.ExpiredFinishedTasks +
                  "\r\n" +
                  "Удалено после истечения времени: " + user.ExpiredDeletedTasks +
                  "\r\n" +
                  "Изменений тасков: " + user.EditedTask);
    }
}