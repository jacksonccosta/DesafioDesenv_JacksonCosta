using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using SQLitePCL;

namespace WebApp_Desafio_FrontEnd
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Batteries_V2.Init();

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}