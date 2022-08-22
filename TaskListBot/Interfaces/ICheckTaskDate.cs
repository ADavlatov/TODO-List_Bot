using TaskListBot.Database;

namespace TaskListBot.Interfaces;

public interface ICheckTaskDate
{
    TaskObject? CheckDate(TaskObject? task);
}