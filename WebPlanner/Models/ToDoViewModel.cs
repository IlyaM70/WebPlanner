using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebPlanner.Models
{
    public class ToDoViewModel
    {
        public ToDo ToDo { get; set; }
        public string Priority { get; set; }
        public string? Tags { get; set; }
        public UserFile UserFile { get; set; }

    }
}
