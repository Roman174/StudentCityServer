using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Holod.Controllers
{
    [Route("api/")]
    [ApiController]
    public class TestController : ControllerBase
    {
        IConfiguration configuration;
        public TestController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet]
        [Route("test")]
        public IActionResult Test()
        {
            var directoryPhotos = configuration.GetSection("HostelPhotoDirectory").Get<string>();
            var fullFileName = $"{Directory.GetCurrentDirectory()}{directoryPhotos}";
            return Ok(
                Directory.GetFiles(fullFileName)
                );
        }
    }
}