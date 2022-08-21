using Microsoft.Extensions.Caching.Memory;
using TODO_List_Bot.Commands;
using TODO_List_Bot.Commands.AddTaskCommands;
using TODO_List_Bot.Commands.TaskActions.EditTask;
using TODO_List_Bot.Commands.TaskActions.EditTask.Date;
using TODO_List_Bot.Commands.TaskActions.EditTask.Time;
using TODO_List_Bot.Interfaces;
using TODO_List_Bot.TasksType;

namespace TODO_List_Bot;

public static class MethodsExtensions
{
    public static ITaskAction? Do(this TaskObject task, string action)
    {
        if (task is null)
        {
            return null;
        }

        return action switch
        {
            "finish" => new FinishTask(),
            "edit" => new EditMenu(),
            "delete" => new DeleteTask(),
            "taskName" => new EditTaskName(),
            "taskDate" => new EditDate(),
            "taskTime" => new EditTime(),
            "addTask" => new AddTask(),
            _ => null,
        };
    }

    public static ITaskAction? Do(this string str, string action)
    {
        if (str is null)
        {
            return null;
        }

        return action switch
        {
            "taskName" => new EditTaskName(),
            "taskDate" => new EditDate(),
            "taskTime" => new EditTime(),
            "addTask" => new AddTask(),
            _ => null,
        };
    }

    public static ISendReplyKeyboard? SendReplyKeyboard(this IMemoryCache cache, string action)
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

    public static ISetNewTaskParameters? SetParameter(this string str, string parameter)
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

    public static IEditTaskParameters? EditParameter(this string str, string parameter)
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

    public static ICheckTaskDate? CheckTaskDate(this TaskObject task, int state)
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