using Holod.Models.Database;
using Holod.Models.Files;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Holod.Controllers
{
    public class StuffController : Controller
    {
        private readonly DatabaseContext database;
        private readonly IHostingEnvironment hosting;
        private readonly IConfiguration configuration;

        public StuffController(DatabaseContext database, IHostingEnvironment hosting, IConfiguration configuration)
        {
            this.database = database;
            this.hosting = hosting;
            this.configuration = configuration;
        }

        [Authorize]
        public IActionResult Index()
        {
            var hostels = database.Hostels.ToList();
            var posts = database.Post.ToList();

            ViewBag.Hostels = hostels;
            ViewBag.Posts = posts;

            return View();
        }

        [Authorize]
        public async Task<IActionResult> Add(Stuff stuff, IFormFile photo)
        {
            try
            {
                string directoryPhotos = configuration.GetSection("StuffPhotoDirectory").Get<string>();
                string fullFileName = $"{Directory.GetCurrentDirectory()}{directoryPhotos}//{photo.FileName}";

                await new FileSaver().SaveFileAsync(fullFileName, photo);

                stuff.Photo = photo.FileName;

                Post post = database
                    .Post
                    .FirstOrDefault(p => p.Title == Request.Form["post"]);
                stuff.Post = post;

                Hostel hostel = database
                    .Hostels
                    .ToList()
                    .FirstOrDefault(p => p.Title == Request.Form["hostelTitle"]);
                stuff.Hostel = hostel;


                if (database.StudentCities.Count() == 0)
                {
                    StudentCity studentCity = new StudentCity
                    {
                        Photo = "",
                        Stuffs = new List<Stuff>(),
                        Hostels = new List<Hostel>()
                    };

                    studentCity.Stuffs.Add(stuff);
                    database.StudentCities.Add(studentCity);
                }
                else
                {
                    StudentCity studentCity = await database
                        .StudentCities
                        .FirstOrDefaultAsync();

                    studentCity.Stuffs.Add(stuff);
                    database.StudentCities.Update(studentCity);
                }
                await database.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View("Views/Error.cshtml", e.Message);
            }
        }
    }
}
