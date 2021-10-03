using System;
using System.Threading.Tasks;
using Client.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Client
{
    internal class Program
    {
        private readonly CalculatorService _calculatorService;

        public Program(CalculatorService calculatorService) => _calculatorService = calculatorService;

        private static async Task Main(string[] args) =>
            await CreateHostBuilder(args).Build().Services.GetRequiredService<Program>().Run();

        private static IHostBuilder CreateHostBuilder(string[] args) => Host
            .CreateDefaultBuilder(args)
            .ConfigureServices((ctx, services) =>
            {
                services.AddGrpcClient<Calculator.CalculatorClient>(opt => opt.Address = ctx.Configuration.GetValue<Uri>("CalculationServiceAddress"));
                services.AddTransient<CalculatorService>();
                services.AddTransient<Program>();
            })
            .ConfigureLogging(builder => builder.SetMinimumLevel(LogLevel.None));

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