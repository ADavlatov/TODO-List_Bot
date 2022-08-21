namespace TODO_List_Bot.Commands;

public class TaskState
{
    public static int GetTaskState(int year, int month, int day, int hour)
    {
        if (DateTime.Now.Year == year && DateTime.Now.Month == month &&
            DateTime.Now.Day == day && DateTime.Now.Hour == hour)
        {
            return (int)TaskStates.oneNotification;
        }

        if (DateTime.Now.Year == year && DateTime.Now.Month == month &&
            DateTime.Now.Day == day && DateTime.Now.Hour != hour)
        {
            return (int)TaskStates.twoNotifications;
        }

        return (int)TaskStates.threeNotifications;
    }
}

public enum TaskStates
{
    oneNotification = 1, //отправка уведомления только в установленное время
    twoNotifications, //отправка уведомления за час и в установленное время
    threeNotifications //отправка уведомлений за 24 часа, час и установленное время
}