namespace Core.Exceptions;

/// <summary>
/// Исключение, брошенное в случае ошибок валидации данных
/// </summary>
public class ValidationException : Exception
{
    /// <summary>
    /// Название свойства, вызвавшего ошибку валидации
    /// </summary>
    public string Property { get; init; }

    // <summary>
    /// Инициализирует новый экземпляр класса ValidationException с заданным сообщением об ошибке и названием свойства
    /// </summary>
    /// <param name="message">Сообщение об ошибке</param>
    /// <param name="prop">Название свойства, вызвавшего ошибку валидации</param>
    public ValidationException(string message, string prop) : base(message)
    {
        Property = prop;
    }
}