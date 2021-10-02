using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Core.Test
{
    public class ParserTest
    {
        [Theory]
        [MemberData(nameof(Data))]
        public async Task ParseExpression(string expression)
        {
            var result = await Parser.Parse(expression);
            Assert.Equal(typeof(Func<decimal>), result.GetType());
        }

        public static IEnumerable<object[]> Data =>
            new[]
            {
                new object[] { "2 + 3 * 4", },
                new object[] { "(2 + 3) * 4", },
                new object[] { "2 * 8 / 4", },
                new object[] { "2 * (8 / 4)", }
            };
    }
}