using Microsoft.AspNetCore.Mvc;
using Holod.Models.Database;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
            List<Hostel> hostels = database
                    .Hostels
                    .Include(hostel => hostel.Coordinates)
                    .Include(hostel => hostel.Stuffs)
                        .ThenInclude(stuff => stuff.Post)
                    .Include(hostel => hostel.Residents)
                    .ToList();

            var l = new List<Hostel>();
            for (int i = 0; i < 20; i++)
            {
                l.AddRange(hostels);
            }

            return View(l);
        }
    }
}
