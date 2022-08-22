using TaskListBot.Database;
using TaskListBot.Interfaces;
using TaskListBot.Services;

namespace TaskListBot.TaskNotifications;

public class TaskNotifier : IHostedService
{
    private readonly ApplicationContext _db = new();

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Hosted Service работает");
#pragma warning disable CS4014
        GetTask(cancellationToken, _db);
#pragma warning restore CS4014
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task GetTask(CancellationToken cancellationToken, ApplicationContext db)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                foreach (var task in db.UserTasks)
                {
                    ICheckTaskDate? checkTaskDate = task.CheckTaskDate(task.State);
                    if (task.State != 0)
                    {
                        HandleUpdateService.SendTaskNotification(checkTaskDate!.CheckDate(task));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            await Task.Delay(1);
        }
    }
}