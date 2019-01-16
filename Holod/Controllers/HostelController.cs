using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Holod.Models.Database;
using Holod.Models.Files;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Holod.Controllers
{
    public class HostelController : Controller
    {
        private readonly DatabaseContext database;
        private readonly IHostingEnvironment hosting;
        private readonly IConfiguration configuration;

        public HostelController(DatabaseContext database, IHostingEnvironment hosting, IConfiguration configuration)
        {
            this.database = database;
            this.hosting = hosting;
            this.configuration = configuration;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }


        [Authorize]
        public async Task<IActionResult> Add(Hostel hostel, IFormFile photo)
        {
            try
            {
                string directoryPhotos = configuration.GetSection("HostelPhotoDirectory").Get<string>();
                string fullFileName = $"{hosting.ContentRootPath}{directoryPhotos}\\{photo.FileName}";

                await new FileSaver().SaveFileAsync(fullFileName, photo);

                hostel.Photo = photo.FileName;
                
                hostel.Stuffs = new List<Stuff>();
                hostel.Residents = new List<Resident>();

                if(database.StudentCities.Count() == 0)
                {
                    StudentCity studentCity = new StudentCity
                    {
                        Photo = "",
                        Stuffs = new List<Stuff>(),
                        Hostels = new List<Hostel>()
                    };

                    studentCity.Hostels.Add(hostel);
                    database.StudentCities.Add(studentCity);
                }
                else
                {
                    StudentCity studentCity = await database
                        .StudentCities
                        .FirstOrDefaultAsync();

                    studentCity.Hostels.Add(hostel);
                    database.StudentCities.Update(studentCity);
                }
                
                
                await database.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                return View("Views/Error.cshtml", e.Message);
            }            
        }
    }
}