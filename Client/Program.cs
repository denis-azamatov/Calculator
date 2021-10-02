using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Client
{
    internal class Program
    {
        private readonly CalculatorService _calculatorService;

        public Program(CalculatorService calculatorService) => _calculatorService = calculatorService;

        private static async Task Main(string[] args) =>
            await CreateHostBuilder(args).Build().Services.GetRequiredService<Program>().Run();

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddTransient<Program>();
                    services.AddTransient<CalculatorService>();
                    services.Configure<AppSettings>(context.Configuration.GetSection("AppSettings"));
                });

        /// <summary>
        /// Метод запускающий калькулятор
        /// </summary>
        private async Task Run()
        {
            while (true)
            {
                Console.Write("Введите выражение: ");
                var input = Console.ReadLine();

                await _calculatorService.Calculate(input);

                Console.WriteLine("Чтобы продолжить нажмите (Y), чтобы выйти нажмите любую другую.");
                var key = Console.ReadKey(true);

                if (key.Key != ConsoleKey.Y)
                    return;
            }
        }
    }
}