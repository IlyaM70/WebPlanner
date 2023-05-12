namespace WebPlanner.Models
{
    // прикрепляемый файл
    public class UserFile
    {
        public int Id { get; set; }
        public string FileLink { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }

        public UserFile()
        {
            FileLink= string.Empty;
        }
    }
}
