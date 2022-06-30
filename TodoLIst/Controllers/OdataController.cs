using Microsoft.AspNetCore.Mvc;
using System.Web.Http;
using System.Web.Http.OData;
using TodoLIst.Models;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;

namespace TodoLIst.Controllers
{
    [System.Web.Http.Route("api/[controller]")]
    public class OdataController : ODataController
    {
        ApplicationContext db;

        public OdataController(ApplicationContext db)
        {
            this.db = db;
        }

        //public IActionResult Index()
        //{
        //    //return View();
        //}

        [EnableQuery]
        [HttpGet("get_todolist")]
        [ProducesResponseType(typeof(IEnumerable<TodoEntry>), StatusCodes.Status200OK)]
        public List<TodoEntry> GetTodoList()
        {
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
            return db.TodoEntries.ToList();
        }

        //[EnableQuery]
        //public SingleResult<Product> Get([FromODataUri] int key)
        //{
        //    IQueryable<Product> result = db.Products.Where(p => p.Id == key);
        //    return SingleResult.Create(result);
        //}









    }








}
