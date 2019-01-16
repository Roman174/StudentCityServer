using Holod.Models.Database;
using Microsoft.AspNetCore.Hosting;
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

            string hostelDirectory = configuration
                .GetSection("HostelPhotoDirectories")
                .ToString();

            hostels = hostels
                .Select(hostel =>
                {
                    hostel.Photo = $"{host}/images/hostels/{hostel.Photo}";
                    return hostel;
                })
                .ToList();

            return Ok(hostels);
        }
    }
}