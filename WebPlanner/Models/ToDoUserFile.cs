namespace WebPlanner.Models
{
    // связь Todo с файлом
    public class ToDoUserFile
    {
        public int Id { get; set; }
        public int ToDoId { get; set; }
        public ToDo? ToDo { get; set; }
        public int FileId { get; set; }
        public UserFile? File{ get; set; }

        public ToDoUserFile() { }
    }
}
