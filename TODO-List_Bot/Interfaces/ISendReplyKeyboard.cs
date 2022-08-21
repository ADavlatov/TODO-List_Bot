using Telegram.Bot;
using Telegram.Bot.Types;

namespace TODO_List_Bot.Interfaces;

public interface ISendReplyKeyboard
{ 
    Task SendReplyKeyboard(ITelegramBotClient bot, Message message, int year = 0, int month = 0, int day = 0, int hour = 0, int minutes = 0);
}