namespace WebPlanner.Models
{
    public class Priority
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Priority()
        {
            Name= string.Empty;
        }

        public Priority(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
