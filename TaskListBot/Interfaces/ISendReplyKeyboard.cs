using Telegram.Bot;
using Telegram.Bot.Types;

namespace TaskListBot.Interfaces;

public interface ISendReplyKeyboard
{ 
    Task SendReplyKeyboard(ITelegramBotClient? bot, Message message, int year = 0, int month = 0, int day = 0, int hour = 0);
}