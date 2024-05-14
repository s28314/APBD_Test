﻿namespace TestProject.Models;

public class Task
{
    public int IdTask { get; set; }
    public string Name { get; set; }
    public string Description  { get; set; }
    public DateTime DeadLine { get; set; }
    public int IdProject { get; set; }
    public int IdTaskType { get; set; }
    public int IdAssignedTo { get; set; }
    public int IdCreator { get; set; }

}