using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using WebApplication.Models;

namespace WebApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var x = CreateHostBuilder(args).Build();
            x.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });


    }
}
