using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Holod
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseWebRoot("wwwroot")
                /*.UseUrls("http://192.168.1.46")*/;
    }
}
