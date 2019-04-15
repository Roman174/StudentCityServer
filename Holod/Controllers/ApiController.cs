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
        [Route("hostels/list")]
        public IActionResult ListHostels()
        {
            List<Hostel> hostels;
            string host = $"{Request.Scheme}://{Request.Host.Host}";
            try
            {
                hostels = database
                    .Hostels
                    .Include(hostel => hostel.Coordinates)
                    .Include(hostel => hostel.Stuffs)
                        .ThenInclude(stuff => stuff.Post)
                    .Include(hostel => hostel.Residents)
                    .ToList();
            }
            catch(ArgumentNullException e)
            {
                return BadRequest("object of hostels in database is null");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }

            hostels = hostels
                .Select(hostel =>
                {
                    hostel.Photo = $"{host}/images/hostels/{hostel.Photo}";
                    hostel.Stuffs = hostel.Stuffs.Select( stuff =>
                    {
                        stuff.Photo = $"{host}/images/stuffs/{hostel.Photo}";
                        return stuff;
                    })
                    .ToList();
                    return hostel;
                })
                .ToList();

            return Ok(hostels);
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