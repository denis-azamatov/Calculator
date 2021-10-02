using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Core.Test
{
    public class CalculatorTest
    {
        [Theory]
        [MemberData(nameof(Data))]
        public async Task CalculateExpressions(string expression, decimal expected)
        {
            var calculator = new Calculator();
            var result = await calculator.Calculate(expression);
            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> Data =>
            new List<object[]>
            {
                new object[] { "2 + 3 * 4", 14 },
                new object[] { "(2 + 3) * 4", 20 },
                new object[] { "2 * 8 / 4", 4 },
                new object[] { "2 * (8 / 4)+-2 /10", 3.8 }
            };
    }
}