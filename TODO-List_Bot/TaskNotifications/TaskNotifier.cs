using TODO_List_Bot.Services;

namespace TODO_List_Bot;

public class TaskNotifier : IHostedService
{
    private ApplicationContext db = new();

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Hosted Service работает");
        GetTask(cancellationToken, db);
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
                HandleUpdateService.SendTaskNotification(TaskChecker.CheckTaskDate(db));
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