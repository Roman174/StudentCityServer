using Microsoft.AspNetCore.Mvc;
using Holod.Models.Database;
using Microsoft.AspNetCore.Authorization;

namespace Holod.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly DatabaseContext database;

        public HomeController(DatabaseContext database)
        {
            this.database = database;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
