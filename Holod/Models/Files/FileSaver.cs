using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace Holod.Models.Files
{
    public class FileSaver
    {
        public async Task SaveFileAsync(string fullFileName, IFormFile file)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                byte[] data = memoryStream.ToArray();

                using (FileStream stream = new FileStream(fullFileName, FileMode.Create, FileAccess.Write))
                {
                    await stream.WriteAsync(data, 0, data.Length);
                    await stream.FlushAsync();
                }
            }
        }
    }
}
