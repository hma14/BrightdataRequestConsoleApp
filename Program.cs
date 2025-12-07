using BrightdataRequestConsoleApp;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace BrightdataRequestConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var settings = config.GetSection("BrightData").Get<BrightDataSettings>();

            var client = new BrightDataClient(settings.ApiToken);

            // Now call your methods:
            var zone = "serp_api2";
            var url = "https://www.google.com/search?q=pizza";
            //JsonElement result = await client.RunAsync(zone, url);

            var result = await client.RunAsync(zone, url, "json");


            // Convert JsonElement to string
           // var html = result.GetRawText();

            Console.WriteLine(result);
        }
    }
}