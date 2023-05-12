using Microsoft.EntityFrameworkCore;
using WebPlanner.Models;

namespace WebPlanner.Utilities
{
    public static class DbInitializer
    {
        public static void Initilize(IApplicationBuilder app)
        {
            using var services = app.ApplicationServices.CreateScope();
            var db = services.ServiceProvider.GetRequiredService<AppDbContext>();

            if (!db.Users.Any())
            {
                db.Users.Add(new User { Name="admin",Password="admin",Email="admin@admin.com"});
            }

            if (!db.Tags.Any())
            {
                db.Tags.Add(new Tag { Name = "работа"});
                db.Tags.Add(new Tag { Name = "учеба"});
                db.Tags.Add(new Tag { Name = "личное"});
                db.Tags.Add(new Tag { Name = "покупки"});
                db.Tags.Add(new Tag { Name = "здоровье"});
                db.Tags.Add(new Tag { Name = "фитнес"});
                db.Tags.Add(new Tag { Name = "свободноевремя"});
            }

            db.SaveChanges();

            if (!db.ToDos.Where(td=>td.User.Name== "admin").Any())
            {
                db.ToDos.Add(new ToDo
                {
                    Name = "Сделать домашнюю работу",
                    Description = "домашняя работа, курсовой",
                    Completed = true,
                    UserId = 1,
                    Deadline = DateTime.Now.AddDays(1),
                    PriorityId = 1
                });

                db.ToDos.Add(new ToDo
                {
                    Name = "Зарядка",
                    Description = "Приседания, отжимания",
                    Completed = false,
                    UserId = 1,
                    Deadline = DateTime.Now.AddDays(2),
                    PriorityId = 4
                });

                db.ToDos.Add(new ToDo
                {
                    Name = "Убраться",
                    Description = "Помыть полы, убраться на кухне",
                    Completed = false,
                    UserId = 1,
                    Deadline = DateTime.Now.AddDays(2),
                    PriorityId = 3
                });

                db.ToDos.Add(new ToDo
                {
                    Name = "Купить продукты",
                    Description = "Огурцы,помидоры,бананы,яблоки",
                    Completed = false,
                    UserId = 1,
                    Deadline = DateTime.Now.AddDays(3),
                    PriorityId = 2
                });

                db.SaveChanges();

                db.ToDoTag.Add(new ToDoTag
                {
                    TagId = db.Tags.FirstOrDefault(t => t.Name == "учеба").Id,
                    ToDoId = db.ToDos.FirstOrDefault(td => td.Name == "Сделать домашнюю работу").Id
                });

                db.ToDoTag.Add(new ToDoTag
                {
                    TagId = db.Tags.FirstOrDefault(t => t.Name == "фитнес").Id,
                    ToDoId = db.ToDos.FirstOrDefault(td => td.Name == "Зарядка").Id
                });

                db.ToDoTag.Add(new ToDoTag
                {
                    TagId = db.Tags.FirstOrDefault(t => t.Name == "личное").Id,
                    ToDoId = db.ToDos.FirstOrDefault(td => td.Name == "Убраться").Id
                });


                db.ToDoTag.Add(new ToDoTag
                {
                    TagId = db.Tags.FirstOrDefault(t => t.Name == "покупки").Id,
                    ToDoId = db.ToDos.FirstOrDefault(td => td.Name == "Купить продукты").Id
                });
                db.ToDoTag.Add(new ToDoTag
                {
                    TagId = db.Tags.FirstOrDefault(t => t.Name == "личное").Id,
                    ToDoId = db.ToDos.FirstOrDefault(td => td.Name == "Купить продукты").Id
                });
            }

            db.SaveChanges();

        }
    }
}
