using System;
using System.Threading.Tasks;

namespace Client.Services
{
    /// <summary>
    /// Сервис калькулятора
    /// </summary>
    public class CalculatorService
    {
        private readonly Calculator.CalculatorClient _client;

        public CalculatorService(Calculator.CalculatorClient client) => _client = client;

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
    }
}