using TODO_List_Bot.Interfaces;

namespace TODO_List_Bot.TasksType;

public class TwoNotifications : ICheckTaskDate
{
    public TaskObject CheckDate(TaskObject task)
    {
        if (task.Date == DateOnly.FromDateTime(DateTime.Now) &&
            task.Time.Hour == (DateTime.Now.Hour + 1) &&
            task.Time.Minute == DateTime.Now.Minute)
        {
            return task;
        }

        return null;
    }
}