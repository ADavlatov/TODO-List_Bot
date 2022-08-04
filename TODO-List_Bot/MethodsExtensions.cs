using Microsoft.Extensions.Caching.Memory;
using TODO_List_Bot.Commands;
using TODO_List_Bot.Commands.AddTaskCommands;
using TODO_List_Bot.Interfaces;

namespace TODO_List_Bot;

public static class MethodsExtensions
{
    public static ICommand? Do(this TaskObject task, string action) {
        if (task is null) {
            return null;
        }
        return action switch {
            "finish" => new FinishTask( ),
            "edit" => new EditTask( ),
            "delete" => new DeleteTask( ),
            "taskName" => new EditTaskName( ),
            "taskDate" => new EditTaskDate( ),
            "taskTime" => new EditTaskTime( ),
            _ => null,
        };
    }

    public static IAddTaskCommand? Do(this IMemoryCache cache, string action)
    {
        if (cache is null) {
            return null;
        }
        return action switch {
            "setTaskName" => new SetTaskName( ),
            "setTaskMonth" => new SetTaskMonth( ),
            "setTaskDay" => new SetTaskDay( ),
            "setTaskHour" => new SetTaskHour( ),
            "setTaskMinute" => new SetTaskMinute( ),
            _ => null,
        };
    }
}