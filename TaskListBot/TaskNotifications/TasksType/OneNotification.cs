using TaskListBot.Database;
using TaskListBot.Interfaces;

namespace TaskListBot.TaskNotifications.TasksType;

public class OneNotification : ICheckTaskDate
{
    public TaskObject? CheckDate(TaskObject? task)
    {
        if (task!.Date.Year == DateTime.Now.Date.Year &&
            task.Date.Month == DateTime.Now.Date.Month &&
            task.Date.Day == DateTime.Now.Date.Day &&
            task.Time.Hour == DateTime.Now.Hour &&
            task.Time.Minute == DateTime.Now.Minute)
        {
            return task;
        }

        return null;
    }
}