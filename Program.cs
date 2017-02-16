using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;


namespace tour_of_heroes_1
{
    public class Program
    {
        public static void Main(string[] args)
        {            
            var urlArg = "--server.urls=";
            var url = args.Where(item =>
                        item.StartsWith(urlArg, StringComparison.CurrentCultureIgnoreCase))
                        .Select(item => item.Replace(urlArg, ""))
                        .FirstOrDefault();
            
            var webHostBuilder = new WebHostBuilder()
                .UseKestrel()
                .UseWebRoot("dist")
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseApplicationInsights();

            if (!string.IsNullOrWhiteSpace(url))
            {
                Console.WriteLine($"command line arg: ${url}");
                webHostBuilder = webHostBuilder.UseUrls(url);
            }

            var host = webHostBuilder.Build();
            
            

            host.Run();
        }
    }
}
