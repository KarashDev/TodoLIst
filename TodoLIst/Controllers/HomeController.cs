using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System.Diagnostics;
using TodoLIst.Models;
using TodoLIst.RabbitMQ;

namespace TodoLIst.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        IRabbitMqService mqService;
        ApplicationContext db;

        // В контроллер ставим контекст подключения к базе 
        public HomeController(ApplicationContext context, ILogger<HomeController> logger, IRabbitMqService mqService)
        {
            _logger = logger;
            db = context;
            this.mqService = mqService;
        }

        public IActionResult Index()
        {
            // На случай, если отсутствуют заметки при открытии проекта - создаются новые. По-хорошему, необходимо
            // такую логику писать не здесь, а на старте программы, например в program.cs, однако это стало сильно сложно с тех пор
            // как в новой версии .net убрали startup с configureServices, можно создать startup вручную, но это костыльно
            if (!db.TodoEntries.Any())
            {
                db.TodoEntries.AddRange(
                   new TodoEntry
                   {
                       Text  = "Сходить в магазин",
                       isHighPriority = true,
                   },
                   new TodoEntry
                   {
                       Text  = "Написать заметки",
                       isHighPriority = false,
                   },
                   new TodoEntry
                   {
                       Text  = "Полюбить C#",
                       isHighPriority = true,
                   },
                   new TodoEntry
                   {
                       Text  = "Неполюбить Assembler",
                       isHighPriority = false,
                   }
               );

                db.SaveChanges();
            }

            return View(db.TodoEntries.ToList());
        }


        public ActionResult Create()
        {
            try
            {
                return View("~/Views/Home/Create.cshtml");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult CreateEntry(TodoEntry entry)
        {
            try
            {
                db.TodoEntries.Add(entry);

                mqService.SendMessage(entry, "CREATED");

                db.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(TodoEntry entry)
        {
            try
            {
                return View("~/Views/Home/Edit.cshtml", entry);
            }
            catch
            {
                return View();
            }
        }

        public ActionResult EditEntry(TodoEntry editedEntry)
        {
            try
            {
                var entryToEdit = db.TodoEntries.FirstOrDefault(e => e.Id == editedEntry.Id);
                entryToEdit.Text = editedEntry.Text;
                entryToEdit.isHighPriority = editedEntry.isHighPriority;

                mqService.SendMessage(entryToEdit, "EDITED");

                db.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult DeleteEntry(TodoEntry entry)
        {
            try
            {
                if (db.TodoEntries.Contains(entry))
                {
                    mqService.SendMessage(entry, "DELETED");

                    db.TodoEntries.Remove(entry);

                    db.SaveChanges();
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}