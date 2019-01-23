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

        [HttpGet]
        [Route("hostels/coordinates")]
        public IActionResult ListCoordinates()
        {
            Dictionary<string, Coordinates> coordinates = new Dictionary<string, Coordinates>();
            string host = $"{Request.Scheme}://{Request.Host.Host}";
            try
            {
                List<Hostel> hostels;

                hostels = database
                    .Hostels
                    .Include(hostel => hostel.Coordinates)
                    .Include(hostel => hostel.Stuffs)
                        .ThenInclude(stuff => stuff.Post)
                    .Include(hostel => hostel.Residents)
                    .ToList();

                foreach (Hostel hostel in hostels)
                {
                    coordinates.Add(hostel.Title, hostel.Coordinates);
                }
            }
            catch (ArgumentNullException e)
            {
                return BadRequest("object of hostels in database is null");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok(coordinates);
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