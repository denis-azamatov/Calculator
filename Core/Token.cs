namespace Core
{
    /// <summary>
    /// Типы токенов из которых состоят выражения
    /// </summary>
    public enum Token
    {
        /// <summary>
        /// Конец выражения
        /// </summary>
        End,
        
        /// <summary>
        /// Знак сложения
        /// </summary>
        Add,
        
        /// <summary>
        /// Знак вычитания
        /// </summary>
        Subtract,
        
        /// <summary>
        /// Знак умножения
        /// </summary>
        Multiply,
        
        /// <summary>
        /// Знак деления
        /// </summary>
        Divide,
        
        /// <summary>
        /// Открывающая скобка
        /// </summary>
        OpenParens,
        
        /// <summary>
        /// Закрывающая скобка
        /// </summary>
        CloseParens,
        
        /// <summary>
        /// Число
        /// </summary>
        Number
    }
}