using User = TaskListBot.Database.User;

namespace TaskListBot.Database;

public record TaskObject()
{
    //@TODO сделать через приватный конструктор и публичный метод
    public long Id { get; set; }
    public long? UserId { get; set; }
    public long? ChatId { get; set; }

    public User? User { get; set; }
    public string? Name { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }
    public int State { get; set; }
}