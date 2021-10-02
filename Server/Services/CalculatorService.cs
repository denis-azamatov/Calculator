using System;
using System.Threading.Tasks;
using Grpc.Core;

namespace Server.Services
{
    public class CalculatorService : Calculator.CalculatorBase
    {
        private readonly Core.Calculator _calculator;

        public CalculatorService(Core.Calculator calculator)
        {
            _calculator = calculator;
        }

        public override async Task<CalculateResponse> Calculate(CalculateRequest request, ServerCallContext context)
        {
            try
            {
                var result = await _calculator.Calculate(request.Expression);
                return new CalculateResponse { Result = result, IsSuccess = true };
            }
            catch (Exception e)
            {
                return new CalculateResponse { IsSuccess = false, Message = e.Message };
            }
        }
    }
}