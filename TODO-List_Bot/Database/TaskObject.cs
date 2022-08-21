using System.Security.Cryptography;
using Telegram.Bot.Types;
using TODO_List_Bot.Commands;
using TODO_List_Bot.Interfaces;

namespace TODO_List_Bot;

public record TaskObject()
{
    public long Id { get; set; }
    public long? UserId { get; set; }
    public long? ChatId { get; set; }

    public User User { get; set; }
    public string Name { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }
    public int State { get; set; }
}
