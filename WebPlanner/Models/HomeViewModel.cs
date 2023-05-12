namespace WebPlanner.Models
{
    public class HomeViewModel
    {
        public List<ToDoViewModel> ToDos { get; set; }
        public string OrderBy { get; set; }
        public string SearchBy { get; set; }
        public string? SearchString { get; set; }
        public DateTime? SearchByDate { get; set; }

        public HomeViewModel()
        {
           ToDos = new List<ToDoViewModel>();
           OrderBy = "Deadline";
           SearchBy= string.Empty;
           SearchString = string.Empty;
        }
    }
}
