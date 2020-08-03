using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace ArtShareApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls("http://*:5002", "https://*:5003");
                    webBuilder.ConfigureKestrel(opt => {
                        opt.Limits.MaxRequestBodySize = long.MaxValue;
                        
                    });
                });
    }
}
