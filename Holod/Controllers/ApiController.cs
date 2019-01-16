using Holod.Models.Database;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Holod.Controllers
{
    [Route("api")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly DatabaseContext database;

        public ApiController(DatabaseContext database)
        {
            this.database = database;
        }

        [HttpGet]
        [Route("hostels/list")]
        public IActionResult ListHostels()
        {
            List<Hostel> hostels = new List<Hostel>
            {
                new Hostel
                {
                    Title = "Общежитие 7.1",
                    Address = "asdasdasd",
                    NumberFloors = 9,
                    NumberStudents = 1000,
                    Phone = "89227093130",
                    Photo = "3333"
                },

                new Hostel
                {
                    Title = "Общежитие 7.2",
                    Address = "Ленина 80",
                    NumberFloors = 9,
                    NumberStudents = 1000,
                    Phone = "89227093130",
                    Photo = "3333"
                }
            };

            return Ok(hostels);
        }
    }
}