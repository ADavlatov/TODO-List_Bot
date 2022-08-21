using TODO_List_Bot.Interfaces;

namespace TODO_List_Bot;

public class TaskChecker
{
    public static TaskObject? CheckTaskDate(ApplicationContext db)
    {
        foreach (var task in db.UserTasks)
        {
            ICheckTaskDate? checkTaskDate = task.CheckTaskDate(task.State);
            return checkTaskDate.CheckDate(task);
        }

        return null;
    }
}