namespace WebPlanner.Models
{
    // связь todo с приоритетом
    public class ToDoPriority
    {
        public int Id { get; set; }
        public int ToDoId { get; set; }
        public ToDo? ToDo { get; set; }
        public int PriorityId { get; set; }
        public Priority? Priority { get; set; }

        public ToDoPriority() { }
    }
}
