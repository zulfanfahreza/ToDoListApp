﻿namespace ToDoListApp.Models
{
    public class ToDoItemModel : BaseEntityModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool IsComplete { get; set; }
    }
}
