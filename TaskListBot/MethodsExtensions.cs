using Microsoft.Extensions.Caching.Memory;
using TaskListBot.Database;
using TaskListBot.Interfaces;
using TaskListBot.TaskNotifications.TasksType;

namespace TaskListBot;

public static class MethodsExtensions
{
    public static ITaskAction? Do(this TaskObject? task, string action)
    {
        if (task is null)
        {
            return null;
        }

        return action switch
        {
            "finish" => new Commands.TaskActions.FinishTask(),
            "edit" => new Commands.TaskActions.EditTask.EditMenu(),
            "delete" => new Commands.TaskActions.DeleteTask(),
            "taskName" => new Commands.TaskActions.EditTask.Name(),
            "taskDate" => new Commands.TaskActions.EditTask.Date.EditDate(),
            "taskTime" => new Commands.TaskActions.EditTask.Time.EditTime(),
            "addTask" => new Commands.AddNewTask.AddTask(),
            _ => null
        };
    }

    public static ITaskAction? Do(this string? str, string action)
    {
        if (str is null)
        {
            return null;
        }

        return action switch
        {
            "taskName" => new Commands.TaskActions.EditTask.Name(),
            "taskDate" => new Commands.TaskActions.EditTask.Date.EditDate(),
            "taskTime" => new Commands.TaskActions.EditTask.Time.EditTime(),
            "addTask" => new Commands.AddNewTask.AddTask(),
            _ => null
        };
    }

    public static ISendReplyKeyboard? SendReplyKeyboard(this IMemoryCache? cache, string action)
    {
        if (cache is null)
        {
            return null;
        }

        return action switch
        {
            "year" => new Commands.ReplyKeyboards.Year(),
            "month" => new Commands.ReplyKeyboards.Month(),
            "day" => new Commands.ReplyKeyboards.Day(),
            "hour" => new Commands.ReplyKeyboards.Hour(),
            "minutes" => new Commands.ReplyKeyboards.Minutes(),
            _ => null,
        };
    }

    public static ISetNewTaskParameters? SetParameter(this string? str, string parameter)
    {
        if (str is null)
        {
            return null;
        }

        return parameter switch
        {
            "name" => new Commands.AddNewTask.SetParameters.Name(),
            "year" => new Commands.AddNewTask.SetParameters.Date.Year(),
            "month" => new Commands.AddNewTask.SetParameters.Date.Month(),
            "day" => new Commands.AddNewTask.SetParameters.Date.Day(),
            "hour" => new Commands.AddNewTask.SetParameters.Time.Hour(),
            "minutes" => new Commands.AddNewTask.SetParameters.Time.Minutes(),
            _ => null
        };
    }

    public static IEditTaskParameters? EditParameter(this string? str, string parameter)
    {
        if (str is null)
        {
            return null;
        }

        return parameter switch
        {
            "year" => new Commands.TaskActions.EditTask.Date.Year(),
            "month" => new Commands.TaskActions.EditTask.Date.Month(),
            "day" => new Commands.TaskActions.EditTask.Date.Day(),
            "hour" => new Commands.TaskActions.EditTask.Time.Hour(),
            "minutes" => new Commands.TaskActions.EditTask.Time.Minutes(),
            _ => null
        };
    }

    public static ICheckTaskDate? CheckTaskDate(this TaskObject? task, int state)
    {
        if (task is null)
        {
            return null;
        }

        return state switch
        {
            1 => new OneNotification(),
            2 => new TwoNotifications(),
            3 => new ThreeNotifications(),
            _ => null
        };
    }
}