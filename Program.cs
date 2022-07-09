using System.Text;
using FinalAdoSoap;
using Microsoft.AspNetCore;
using Serilog;

public class Program{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration().WriteTo.File("WebServicesEnrollment.out", Serilog.Events.LogEventLevel.Debug, "{Message:lj}{NewLine}", encoding: Encoding.UTF8).CreateLogger();
        CreateWebHostBuilder(args).Build().Run();
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args)=>
        WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();
}