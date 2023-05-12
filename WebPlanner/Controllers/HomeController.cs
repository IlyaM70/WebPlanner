using Microsoft.AspNetCore.Mvc;
using News.Models;
using System.Diagnostics;
using WebPlanner.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Protocol;
using Microsoft.AspNetCore.Html;
using System.Text.RegularExpressions;
using NuGet.Packaging;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Azure;

namespace WebPlanner.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext db;

        public HomeController(ILogger<HomeController> logger, AppDbContext db)
        {
            _logger = logger;
            this.db = db;
        }

        private List<Priority> Priorities = new List<Priority>()
        {
            new Priority { Id=1, Name = "1-Высший" },
            new Priority { Id=2, Name = "2-Высокий" },
            new Priority { Id=3, Name = "3-Средний" },
            new Priority { Id=4, Name = "4-Низкий" },
            new Priority { Id=5, Name = "5-Низший" }
        };

        [HttpGet]
        public IActionResult Index(HomeViewModel viewModel)
        {
            var userId = GetUserId();
            viewModel.ToDos = GetToDoList(viewModel, userId);
            return View(viewModel);
        }

        


        [HttpGet]
        public IActionResult Add(ToDoViewModel viewModel)
        {
            ViewBag.Priorities = new SelectList(Priorities,"Id","Name");
            ViewBag.Tags = GetAllTagsHtml();
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Add(ToDoViewModel viewModel, IFormCollection formValues)
        {
            ViewBag.Priorities = new SelectList(Priorities, "Id", "Name");
            var userId = GetUserId();
            viewModel.ToDo.UserId = userId; 


            // добавляем задачу
            db.ToDos.Add(viewModel.ToDo);
            db.SaveChanges();

            // добавляем связь с тегами
            if (viewModel.Tags != null)
            {
                var tags = viewModel.Tags.Split(',');
                AddTagsForToDo(viewModel.ToDo, tags);
            }

            // file
            ViewData["ValidateMessage"] = "Задача " + viewModel.ToDo.Name + " добавлена";
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Update(ToDoViewModel viewModel, int id, string priority)
        {
            ViewBag.Priorities = new SelectList(Priorities, "Id", "Name");
            // выпадающие теги
            ViewBag.Tags = GetAllTagsHtml();

            var toDo = db.ToDos.FirstOrDefault(td => td.Id == id);
            viewModel.ToDo = toDo;
            viewModel.Priority = priority;
            // теги у задачи
            var tags = GetToDoTags(toDo);

            string tagsStr = String.Join(",", tags);

            viewModel.Tags = tagsStr;

            // / file

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Update(ToDoViewModel viewModel, IFormCollection formValues)
        {
            ViewBag.Priorities = new SelectList(Priorities, "Id", "Name");
            // выпадающие теги
            ViewBag.Tags = GetAllTagsHtml();

            var userId = GetUserId();
            viewModel.ToDo.UserId = userId;

            db.ToDos.Update(viewModel.ToDo);
            db.SaveChanges();

            //изменить связь с тегами
            // старые теги
            string[] oldTags = GetToDoTags(viewModel.ToDo).ToArray();
            // новые теги
            string[] tags = new string[] { };
             if (viewModel.Tags !=null )
            {
                tags = viewModel.Tags.Split(',');
            }
            
            // теги для удаления
            string[] tagsToDelete = oldTags.Except(tags).ToArray();
            // теги для добавления
            string[] tagsToAdd = tags.Except(oldTags).Except(tagsToDelete).ToArray();
            // удалить теги
            foreach (var tag in tagsToDelete)
            {
                var toDoTagToDelete = db.ToDoTag
                    .Include(tdt=>tdt.Tag)
                    .FirstOrDefault(tdt => tdt.ToDoId == viewModel.ToDo.Id && tdt.Tag.Name == tag);

                db.ToDoTag.Remove(toDoTagToDelete);
                db.SaveChanges();
            }
            // добавить теги
            AddTagsForToDo(viewModel.ToDo, tagsToAdd);

            // / file
            ViewData["ValidateMessage"] = "Задача " + viewModel.ToDo.Name + " обновлена";
            return View(viewModel);
        }

        public async Task<RedirectToActionResult> Delete(int id)
        {
            var toDelete = await db.ToDos.FindAsync(id);
            if (toDelete != null)
            {
                //удалить задачу
                db.ToDos.Remove(toDelete);
                db.SaveChanges();

                //удалить связь между тегами и задачей
                var toDoTagsToDelete = db.ToDoTag.Where(tdt => tdt.ToDoId == toDelete.Id).ToList();
                db.ToDoTag.RemoveRange(toDoTagsToDelete);
                db.SaveChanges();


                TempData["Feedback"] = "Задача "+toDelete.Name+" удалена";
            }
            else
            {
                TempData["Feedback"] = "Задача не найдена";
            }
            return RedirectToAction("Index");
        } 

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login","Access");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        //////////////  Функции

        // Список задач для главной страницы
        public List<ToDoViewModel> GetToDoList(HomeViewModel viewModel, int userId)
        {
            string orderBy = viewModel.OrderBy;
            string searchBy = viewModel.SearchBy;
            string searchString = viewModel.SearchString;
            var query = db.ToDos.Where(td => td.UserId == userId);
            List<ToDo> toDoList = new List<ToDo>();


            switch (orderBy)
            {
                case "Deadline":
                    query = query.OrderBy(td => td.Deadline);
                    break;
                case "DeadlineDescend":
                    query = query.OrderByDescending(td => td.Deadline);
                    break;
                case "Name":
                    query = query.OrderBy(td => td.Name);
                    break;
                case "NameDescend":
                    query = query.OrderByDescending(td => td.Name);
                    break;
                case "Priority":
                    query = query.OrderBy(td => td.PriorityId);
                    break;
                case "PriorityDescend":
                    query = query.OrderByDescending(td => td.PriorityId);
                    break;
                case "Completed": // сначало не выполненные
                    query = query.OrderBy(td => td.Completed);
                    break;
                case "CompletedDescend": // сначало выполненные
                    query = query.OrderByDescending(td => td.Completed);
                    break;
                default:
                    break;
            }

            if (viewModel.SearchByDate != null)
            {
                query = query.Where(td => td.Deadline.Date == viewModel.SearchByDate.Value.Date);
            }

            toDoList = query.ToList();

            foreach (ToDo toDo in toDoList)
            {
                string priority = Priorities.FirstOrDefault(p => p.Id == toDo.PriorityId).Name;

                var tags = GetToDoTags(toDo);

                string tagsStr = String.Join(",", tags);

                viewModel.ToDos.Add(new ToDoViewModel
                {
                    ToDo = toDo,
                    Priority = priority,
                    Tags = tagsStr
                    // file
                });
            }

            if (searchString != string.Empty && searchString != null)
            {
                switch (searchBy)
                {
                    case "Name":
                        viewModel.ToDos = viewModel.ToDos.Where(td => td.ToDo.Name == searchString).ToList();
                        break;
                    case "Tag":
                        searchString = MakeProperTag(searchString);
                        viewModel.ToDos = viewModel.ToDos.Where(td => td.Tags.Contains(searchString)).ToList();
                        break;
                    default:
                        break;
                }
            }

            return viewModel.ToDos;
        }


        public int GetUserId()
        {
            var userId = Convert.ToInt32(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            return userId;
        }
        // html строка со всеми тегами для tagify скрипта
        public HtmlString GetAllTagsHtml()
        {
            var tags = db.Tags.ToList();
            string tagsStr = "";
            foreach (var tag in tags)
            {
                tagsStr += "'" + tag.Name + "'" + ",";
            }
            var tagsHtml = new HtmlString(tagsStr);
            return tagsHtml;
        }

        public List<string> GetToDoTags(ToDo toDo)
        {
            var tags = db.ToDoTag.Include(tdt => tdt.Tag)
                .Where(tdt => tdt.ToDoId == toDo.Id)
                .Select(tdt => tdt.Tag)
                .Select(t => t.Name)
                .ToList();
            return tags;
        }

        public void AddTagsForToDo(ToDo toDo, string[] tags)
        {
            foreach (var tag in tags)
            {
                // привести к требуемому виду
                string properTag = MakeProperTag(tag);
                //var properTag = "";
                //properTag = Regex.Replace(tag, @"\s+", "");
                //properTag = properTag.ToLower();

                var dbTag = db.Tags.FirstOrDefault(tg => tg.Name == properTag);
                if (dbTag != null) // если тег существует добавляем связь
                {
                    db.ToDoTag.Add(new ToDoTag { TagId = dbTag.Id, ToDoId = toDo.Id });
                }
                else
                {
                    var newTag = db.Tags.Add(new Tag { Name = properTag });
                    db.SaveChanges();
                    db.ToDoTag.Add(new ToDoTag { TagId = newTag.Entity.Id, ToDoId = toDo.Id });
                }
            }
            db.SaveChanges();
        }
        public string MakeProperTag(string tag)
        {
            var properTag = "";
            properTag = Regex.Replace(tag, @"\s+", "");
            properTag = properTag.ToLower();
            return properTag;
        }

    }
}