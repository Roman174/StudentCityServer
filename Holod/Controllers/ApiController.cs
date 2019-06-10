using Holod.Models.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Holod.Controllers
{
    [Route("api")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly DatabaseContext database;
        private readonly IConfiguration configuration;

        public ApiController(DatabaseContext database, IConfiguration configuration)
        {
            this.database = database;
            this.configuration = configuration;
        }

        [HttpGet]
        [Route("hostel/info/{hostelName}")]
        public IActionResult HostelInfo(string hostelName)
        {
            if(database.Hostels.Count() == 0)
            {
                return BadRequest("database is empty");
            }

            Hostel hostel = database.Hostels
                .Include(h => h.Coordinates)
                .Include(h => h.Stuffs)
                    .ThenInclude(stuff => stuff.Post)
                .Include(h => h.Residents)
                .ToList()
                .Select(h =>
                {
                    h.Stuffs.ForEach(stuff =>
                    {
                        stuff.Post.Stuffs = null;
                    });
                    return h;
                })
                .FirstOrDefault(h => h.Title == hostelName);

            string host = $"{Request.Scheme}://{Request.Host.Host}";
            hostel.Photo = $"{host}/images/hostels/{hostel.Photo}";
            hostel.Stuffs.ForEach(stuff =>
            {
                stuff.Photo = stuff.Photo = $"{host}/images/stuffs/{stuff.Photo}";
                stuff.Post.Stuffs = null;
            });

            if (hostel is null)
            {
                return StatusCode(404, "hostel not found");
            }
            else
            {
                return Ok(hostel);
            }
        }

        [HttpGet]
        [Route("hostels/list")]
        public IActionResult ListHostels()
        {
            List<Hostel> hostels;
            try
            {
                hostels = database
                    .Hostels
                    .ToList();
                return Ok(hostels);
            }
            catch (ArgumentNullException e)
            {
                return BadRequest("object of hostels in database is null");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("hostels/coordinates/list")]
        public IActionResult ListCoordinates()
        {
            Dictionary<string, Coordinates> coordinates = new Dictionary<string, Coordinates>();
            try
            {
                List<Hostel> hostels = database
                    .Hostels
                    .Include(hostel => hostel.Coordinates)
                    .Include(hostel => hostel.Stuffs)
                        .ThenInclude(stuff => stuff.Post)
                    .Include(hostel => hostel.Residents)
                    .ToList();

                if (hostels is null) return BadRequest("list of hostel in database is empty");

                foreach (Hostel hostel in hostels)
                {
                    coordinates.Add(hostel.Title, hostel.Coordinates);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok(coordinates);
        }
    }
}