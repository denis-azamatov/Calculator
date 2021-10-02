using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace Core
{
    /// <summary>
    /// Считыватель токенов
    /// </summary>
    public class TokenReader : IDisposable
    {
        private readonly TextReader _reader;
        private char _currentChar;

        public TokenReader(string expression)
        {
            _reader = new StringReader(expression);
            NextChar();
            NextToken();
        }

        /// <summary>
        /// Текущий токен
        /// </summary>
        public Token Token { get; private set; }

        /// <summary>
        /// Текущее число
        /// </summary>
        public decimal Number { get; private set; }

        /// <summary>
        /// Считывает следующий токен
        /// </summary>
        public void NextToken()
        {
            while (char.IsWhiteSpace(_currentChar))
            {
                NextChar();
            }

            switch (_currentChar)
            {
                case '\0':
                    Token = Token.End;
                    return;

                case '+':
                    NextChar();
                    Token = Token.Add;
                    return;

                case '-':
                    NextChar();
                    Token = Token.Subtract;
                    return;

                case '*':
                    NextChar();
                    Token = Token.Multiply;
                    return;

                case '/':
                    NextChar();
                    Token = Token.Divide;
                    return;

                case '(':
                    NextChar();
                    Token = Token.OpenParens;
                    return;

                case ')':
                    NextChar();
                    Token = Token.CloseParens;
                    return;
            }

            if (!char.IsDigit(_currentChar) && _currentChar != '.')
                throw new Exception($"Неизвестный символ {_currentChar}");

            var sb = new StringBuilder();
            var haveDecimalPoint = false;
            while (char.IsDigit(_currentChar) || (!haveDecimalPoint && _currentChar == '.'))
            {
                sb.Append(_currentChar);
                haveDecimalPoint = _currentChar == '.';
                NextChar();
            }

            Number = decimal.Parse(sb.ToString(), CultureInfo.InvariantCulture);
            Token = Token.Number;
        }

        /// <summary>
        /// Считывает следующий символ
        /// </summary>
        private void NextChar()
        {
            var ch = _reader.Read();
            _currentChar = ch < 0 ? '\0' : (char)ch;
        }

        public void Dispose()
        {
            _reader.Dispose();
        }
    }
}