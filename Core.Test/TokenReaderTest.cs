using System;
using System.Collections.Generic;
using Xunit;

namespace Core.Test
{
    public class TokenReaderTest
    {
        [Theory]
        [MemberData(nameof(SimpleData))]
        public void ReadTokenSuccess(string expression, decimal a, Token token, decimal b)
        {
            using var reader = new TokenReader(expression);

            var number1 = reader.Number;
            reader.NextToken();
            var oper = reader.Token;
            reader.NextToken();
            var number2 = reader.Number;

            Assert.Equal(a, number1);
            Assert.Equal(token, oper);
            Assert.Equal(b, number2);
        }

        public static IEnumerable<object[]> SimpleData =>
            new[]
            {
                new object[] { "1.0+2.4", 1, Token.Add, 2.4m },
                new object[] { "3.5-2.2", 3.5, Token.Subtract, 2.2m },
                new object[] { "3.5*2.2", 3.5, Token.Multiply, 2.2m },
                new object[] { "3.5/2.2", 3.5, Token.Divide, 2.2m }
            };
    }
}