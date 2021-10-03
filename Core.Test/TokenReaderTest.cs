using System.Collections.Generic;
using Xunit;

namespace Core.Test
{
    public class TokenReaderTest
    {
        [Theory]
        [MemberData(nameof(SimpleData))]
        public void ReadToken(string expression, double a, Token token, double b)
        {
            using var reader = new TokenReader(expression);

            var number1 = reader.Number;
            reader.NextToken();
            var op = reader.Token;
            reader.NextToken();
            var number2 = reader.Number;

            Assert.Equal(a, number1);
            Assert.Equal(token, op);
            Assert.Equal(b, number2);
        }

        public static IEnumerable<object[]> SimpleData =>
            new[]
            {
                new object[] { "1.0+2.4", 1, Token.Add, 2.4 },
                new object[] { "3.5-2.2", 3.5, Token.Subtract, 2.2 },
                new object[] { "3.5*2.2", 3.5, Token.Multiply, 2.2 },
                new object[] { "3.5/2.2", 3.5, Token.Divide, 2.2 }
            };
    }
}