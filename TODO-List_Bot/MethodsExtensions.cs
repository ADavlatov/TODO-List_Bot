using Microsoft.Extensions.Caching.Memory;
using TODO_List_Bot.Commands;
using TODO_List_Bot.Commands.AddTaskCommands;
using TODO_List_Bot.Commands.AddTaskCommands.TaskDate;
using TODO_List_Bot.Commands.AddTaskCommands.TaskTime;
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
            "addTask" => new NewTask(),
            _ => null,
        };
    }

    // public static IAddNewTaskCommands? Do(this IMemoryCache cache, string action)
    // {
    //     if (cache is null) {
    //         return null;
    //     }
    //     return action switch {
    //         "setMonth" => new TaskMonth(),
    //         "setDay" => new TaskDay(),
    //         _ => null,
    //     };
    // }
    
    public static ISendReplyKeyboard? SendReplyKeyboard(this IMemoryCache cache, string action)
    {
        if (cache is null) {
            return null;
        }
        return action switch {
            "month" => new TaskMonth(),
            "day" => new TaskDay(),
            "hour" => new TaskHour(),
            "minutes" => new TaskMinutes(),
            _ => null,
        };
    }
}