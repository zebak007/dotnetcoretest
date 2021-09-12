using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace dotnetcoretest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();

            // Application code should start here.
            string[] words = { "bot", "apple", "apricot" };
            int minimalLength = words
                    .Where(w => w.StartsWith("a"))
                    .Min(w => w.Length);
            Console.WriteLine(minimalLength);   // output: 5

            int[] numbers = { 4, 7, 10 };
            int product = numbers.Aggregate(1, (interim, next) => interim * next);
            Console.WriteLine(product);   // output: 280

            await host.RunAsync();
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args);
    }
}
