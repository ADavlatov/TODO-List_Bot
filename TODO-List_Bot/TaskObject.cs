using System.Security.Cryptography;
using TODO_List_Bot.Commands;
using TODO_List_Bot.Interfaces;

namespace TODO_List_Bot;

public record TaskObject(string Name)
{ 
    public string Name { get; set; } = Name;
    // public DateOnly Date { get; set; }  
    // public TimeOnly Time { get; set; }
}

public static class Extensions
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
            "addTask" => new AddTask( ),
            _ => null,
        };
    }
}