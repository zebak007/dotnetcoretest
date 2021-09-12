using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;


namespace dotnetcoretest
{
    class Program
    {
        private static DownloaderOptions options; 

        private static List<Enrollment> enrollmentList = new List<Enrollment>();
    
        static async Task Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();

            // Application code should start here.

            Console.WriteLine($"options.BearerToken={options.BearerToken}");
            Console.WriteLine($"options.NumberOfMonthsToDownload={options.NumberOfMonthsToDownload}");

            foreach (Enrollment enrollment in enrollmentList)
            {
                Console.WriteLine($"Enrollment number = {enrollment.EnrollmentNumber}");
                Console.WriteLine($"Enrollment path = {enrollment.Path}");
            }

            await host.RunAsync();
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, configuration) =>
                {
                    configuration.Sources.Clear();

                    IHostEnvironment env = hostingContext.HostingEnvironment;

                    configuration
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true);

                    IConfigurationRoot configurationRoot = configuration.Build();

                    options = new DownloaderOptions();

                    configurationRoot.GetSection(nameof(DownloaderOptions))
                                     .Bind(options);
                    
                    var enrollments = configurationRoot.GetSection("Enrollments").GetChildren(); 
                    foreach (var keyValuePair in enrollments) 
                    {
                        Enrollment newEnrollment = new Enrollment()
                        {
                            EnrollmentNumber = keyValuePair.Key,
                            Path = keyValuePair.Value
                        };
                        enrollmentList.Add(newEnrollment);
                    }
                });
    }

    public class DownloaderOptions {
        public string BearerToken {get;set;}
        public int NumberOfMonthsToDownload {get;set;} 
    }

    public class Enrollment {
        public string EnrollmentNumber {get;set;}
        public string Path {get;set;}
    }
}
