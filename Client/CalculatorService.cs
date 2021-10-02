using System;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Microsoft.Extensions.Options;

namespace Client
{
    /// <summary>
    /// Сервис калькулятора
    /// </summary>
    public class CalculatorService : IDisposable
    {
        private readonly GrpcChannel _channel;
        private readonly Calculator.CalculatorClient _client;

        public CalculatorService(IOptions<AppSettings> options)
        {
            var settings = options.Value;
            _channel = GrpcChannel.ForAddress(settings.CalculationServiceAddress);
            _client = new Calculator.CalculatorClient(_channel);
        }

        /// <summary>
        /// Расчитать выражение
        /// </summary>
        public async Task Calculate(string expression)
        {
            try
            {
                var response = await _client.CalculateAsync(new CalculateRequest { Expression = expression });
                Console.WriteLine(response.IsSuccess ? response.Result : response.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void Dispose()
        {
            _channel.Dispose();
        }
    }
}