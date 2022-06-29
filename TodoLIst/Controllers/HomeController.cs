using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System.Diagnostics;
//using System.Web.Http.OData;
using TodoLIst.Models;

namespace TodoLIst.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        ApplicationContext db;

        // В контроллер ставим контекст подключения к базе 
        public HomeController(ApplicationContext context, ILogger<HomeController> logger)
        {
            _logger = logger;
            db = context;
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
                       isActive = true,
                   },
                   new TodoEntry
                   {
                       Text  = "Написать заметки",
                       isActive = false,
                   },
                   new TodoEntry
                   {
                       Text  = "Полюбить C#",
                       isActive = true,
                   },
                   new TodoEntry
                   {
                       Text  = "Возненавидеть JS",
                       isActive = false,
                   }
               );

                db.SaveChanges();
            }
            return View(db.TodoEntries.ToList());
        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                db.TodoEntries.Add(new TodoEntry() { Text=collection["Text"]});

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}

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