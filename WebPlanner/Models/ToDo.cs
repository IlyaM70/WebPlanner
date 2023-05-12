namespace WebPlanner.Models
{
    public class ToDo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Completed { get; set; }
        public DateTime Deadline { get; set; }
        public bool IsSubToDo { get; set; } // является ли подзадачей
        public int UserId { get; set; }
        public User? User { get; set; }
        public int PriorityId { get; set; }

        
        public ToDo() {
            Name = "";
            Description = "";
            Completed = false;
            Deadline = DateTime.Today;
            IsSubToDo = false;
            PriorityId = 1;
        }
    }
}
