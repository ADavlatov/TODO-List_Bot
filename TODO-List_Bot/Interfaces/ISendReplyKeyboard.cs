using Telegram.Bot;
using Telegram.Bot.Types;

namespace TODO_List_Bot.Interfaces;

public interface ISendReplyKeyboard
{ 
    Task SendReplyKeyboard(ITelegramBotClient bot, Message message);
}