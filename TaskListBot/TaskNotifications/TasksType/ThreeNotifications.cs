using TaskListBot.Database;
using TaskListBot.Interfaces;

namespace TaskListBot.TaskNotifications.TasksType;

public class ThreeNotifications : ICheckTaskDate
{
    public TaskObject? CheckDate(TaskObject? task)
    {
        if (task!.Date.Year == DateTime.Now.Year &&
            task.Date.Month == DateTime.Now.Month &&
            task.Date.Day == (DateTime.Now.Day + 1) &&
            task.Time.Hour == DateTime.Now.Hour &&
            task.Time.Minute == DateTime.Now.Minute)
        {
            return task;
        }

        return null;
    }
}