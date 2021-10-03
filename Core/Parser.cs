using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core
{
    /// <summary>
    /// Парсер выражений
    /// </summary>
    public class Parser : IDisposable
    {
        private readonly TokenReader _tokenReader;

        private Parser(TokenReader tokenReader)
        {
            _tokenReader = tokenReader;
        }

        /// <summary>
        /// Парсит выражение
        /// </summary>
        /// <returns>Построеное лямбда выражение</returns>
        private Func<double> ParseExpression()
        {
            var expr = ParseAddSubtract();

            if (_tokenReader.Token != Token.End)
                throw new Exception("Некорректный символ в конце строки");
            
            var lambda = Expression.Lambda(expr, true, null).Compile();

            return (Func<double>)lambda;
        }

        /// <summary>
        /// Парсит выражения сложения и вычитания
        /// </summary>
        /// <returns>Построеное выражение</returns>
        private Expression ParseAddSubtract()
        {
            var lhs = ParseMultiplyDivide();

            while (true)
            {
                ExpressionType? op = _tokenReader.Token switch
                {
                    Token.Add => ExpressionType.Add,
                    Token.Subtract => ExpressionType.Subtract,
                    _ => null
                };

                if (op == null)
                    return lhs;

                _tokenReader.NextToken();

                var rhs = ParseMultiplyDivide();

                lhs = op switch
                {
                    ExpressionType.Add => Expression.Add(lhs, rhs),
                    ExpressionType.Subtract => Expression.Subtract(lhs, rhs),
                    _ => throw new Exception("Неизвестная операция")
                };
            }
        }

        /// <summary>
        /// Парсит выражения умножение и деление
        /// </summary>
        /// <returns>Построеное выражение</returns>
        private Expression ParseMultiplyDivide()
        {
            var lhs = ParseUnary();

            while (true)
            {
                ExpressionType? op = _tokenReader.Token switch
                {
                    Token.Multiply => ExpressionType.Multiply,
                    Token.Divide => ExpressionType.Divide,
                    _ => null
                };

                if (op == null)
                    return lhs;

                _tokenReader.NextToken();

                var rhs = ParseUnary();

                lhs = op switch
                {
                    ExpressionType.Multiply => Expression.Multiply(lhs, rhs),
                    ExpressionType.Divide => Expression.Divide(lhs, rhs),
                    _ => throw new Exception("Неизвестная операция")
                };
            }
        }

        /// <summary>
        /// Парсит унарные выражения
        /// </summary>
        /// <returns>Построеное выражение</returns>
        private Expression ParseUnary()
        {
            switch (_tokenReader.Token)
            {
                case Token.Add:
                    _tokenReader.NextToken();
                    return ParseUnary();
                case Token.Subtract:
                {
                    _tokenReader.NextToken();
                    var rhs = ParseUnary();
                    return Expression.Negate(rhs);
                }
                default:
                    return ParseLeaf();
            }
        }

        /// <summary>
        /// Парсит числа и подвыражения(выражения в скобках)
        /// </summary>
        /// <returns>Построеное выражение</returns>
        private Expression ParseLeaf()
        {
            switch (_tokenReader.Token)
            {
                case Token.Number:
                {
                    var expr = Expression.Constant(_tokenReader.Number);
                    _tokenReader.NextToken();
                    return expr;
                }
                case Token.OpenParens:
                {
                    _tokenReader.NextToken();
                    var expr = ParseAddSubtract();
                    if (_tokenReader.Token != Token.CloseParens)
                        throw new Exception("Не найдена закрывающая скобка");
                    _tokenReader.NextToken();
                    return expr;
                }
                default:
                    throw new Exception($"Некорректный токен: {_tokenReader.Token}");
            }
        }

        /// <summary>
        /// Парсит выражение
        /// </summary>
        /// <param name="expression">Выражение</param>
        /// <returns>Построеное лямбда выражение</returns>
        public static async Task<Func<double>> Parse(string expression)
        {
            using var parser = new Parser(new TokenReader(expression));
            return await Task.Run(() => parser.ParseExpression());
        }

        public void Dispose()
        {
            _tokenReader.Dispose();
        }
    }
}