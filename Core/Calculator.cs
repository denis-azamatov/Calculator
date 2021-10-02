using System.Threading.Tasks;

namespace Core
{
    /// <summary>
    /// Калькулятор
    /// </summary>
    public class Calculator
    {
        /// <summary>
        /// Расчитывает выражение
        /// </summary>
        /// <param name="expression">Выражение</param>
        /// <returns>Результат выражения</returns>
        public async Task<double> Calculate(string expression)
        {
            var expr = await Parser.Parse(expression);
            return expr();
        }
    }
}