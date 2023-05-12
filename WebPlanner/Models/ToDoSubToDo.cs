namespace WebPlanner.Models
{
    // связь между задачами и подзадачами 
    public class ToDoSubToDo
    {
        public int Id { get; set; }
        public int ToDoId { get; set; }
        public ToDo? ToDo { get; set; }
        public int SubToDoId { get; set; }
        public ToDo? SubToDo { get; set; }

        public ToDoSubToDo() { }
    }
}
