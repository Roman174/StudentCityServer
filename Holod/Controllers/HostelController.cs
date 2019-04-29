using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Holod.Models;
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
            try
            {
                this.database = database;
            }
            catch
            {
                this.database = null;
            }
            this.hosting = hosting;
            this.configuration = configuration;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }        

        [Authorize]
        public async Task<IActionResult> Add(Hostel hostel, string latitude, string longitude, IFormFile photo)
        {
            string redirectUrl = $"{Request.Scheme}://{Request.Host.Host}/hostels/index";
            ErrorViewModel errorModel = new ErrorViewModel
            {
                RedirectUrl = redirectUrl
            };

            if (database is null)
            {
                errorModel.Message = "Ошибка при подключении к базе данных";
                return GenereateErrorView(errorModel);
            }

            if (IsEmpty(hostel) || photo is null)
            {
                errorModel.Message = "Введены не все данные";
                return GenereateErrorView(errorModel);
            }

            double numericalLatitude;
            double numericalLongitude;
            try
            {
                numericalLatitude = double.Parse(latitude);
                numericalLongitude = double.Parse(longitude);

                if(numericalLatitude == 0 || numericalLongitude == 0)
                {
                    errorModel.Message = "Ошибка препобразования координат";
                    return GenereateErrorView(errorModel);
                }
            }
            catch (ArgumentNullException)
            {
                errorModel.Message = "Координаты не введены";
                
                return GenereateErrorView(errorModel);
            }
            catch (FormatException)
            {
                errorModel.Message = "Координаты введены неправильно";
                return GenereateErrorView(errorModel);
            }
            catch (OverflowException)
            {
                errorModel.Message = "Ошибка препобразования координат";
                return GenereateErrorView(errorModel);
            }

            string directoryPhotos;
            string fullFileName;
            try
            {
                directoryPhotos = configuration.GetSection("HostelPhotoDirectory").Get<string>();
                fullFileName = $"{hosting.ContentRootPath}{directoryPhotos}\\{photo.FileName}";

                if(directoryPhotos.Equals(string.Empty) || fullFileName.Equals(string.Empty))
                {
                    errorModel.Message = "Отсутствует информация о директории сохранения фотогрпфии";
                    return GenereateErrorView(errorModel);
                }
            }
            catch (NullReferenceException)
            {
                errorModel.Message = "Отсутствует информация о директории сохранения фотогрпфии";
                return GenereateErrorView(errorModel);
            }

            try
            {
                await new FileSaver().SaveFileAsync(fullFileName, photo);
            }
            catch(Exception)
            {
                errorModel.Message = "Ошибка сохранения фотографии";
                return GenereateErrorView(errorModel);
            }

            hostel.Photo = photo.FileName;
            hostel.Coordinates = new Coordinates
            {
                Latitude = numericalLatitude,
                Longitude = numericalLongitude
            };
            hostel.Stuffs = new List<Stuff>();
            hostel.Residents = new List<Resident>();

            if (database.StudentCities.Count() == 0)
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

            try
            {
                await database.SaveChangesAsync();
            }
            catch(DbUpdateException)
            {
                errorModel.Message = "Ошибка сохранения в базе данных";
                return GenereateErrorView(errorModel);
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Проверка введенных данных об общежитии
        /// </summary>
        /// <param name="hostel">
        /// Объект общежития, который был получен из веб-формы
        /// </param>
        private bool IsEmpty(Hostel hostel)
        {
            if (hostel is null)
                return true;

            if (hostel.Title is null || hostel.Phone is null || hostel.Address is null)
                return true;

            if (string.IsNullOrWhiteSpace(hostel.Title) || string.IsNullOrWhiteSpace(hostel.Phone)
                || string.IsNullOrWhiteSpace(hostel.Address) || hostel.NumberFloors == 0
                || hostel.NumberStudents == 0)
                return true;

            return false;
        }

        private ViewResult GenereateErrorView(ErrorViewModel errorModel) => View("Views/Error.cshtml", errorModel);
    }
}