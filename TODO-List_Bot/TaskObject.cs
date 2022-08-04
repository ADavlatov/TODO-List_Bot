using System.Security.Cryptography;
using TODO_List_Bot.Commands;
using TODO_List_Bot.Interfaces;

namespace TODO_List_Bot;

public record TaskObject(string name, DateOnly taskDate, TimeOnly taskTime)
{ 
    public string Name { get; set; } = name;
    public DateOnly Date { get; set; } = taskDate;
    public TimeOnly Time { get; set; } = taskTime;
}
