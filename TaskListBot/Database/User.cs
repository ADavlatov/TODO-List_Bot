namespace TaskListBot.Database;

public record User()
{
    //@TODO сделать через приватный конструктор и публичный метод
    public long Id { get; set; }
    public long? UserId { get; set; }
    public List<TaskObject> UserTasks { get; set; } = new();
    public int FinishedTasks { get; set; }
    public int DeletedTasks { get; set; }
    public int ExpiredFinishedTasks { get; set; }
    public int ExpiredDeletedTasks { get; set; }
    public int EditedTask { get; set; }
    public int AddedTasks { get; set; }
};