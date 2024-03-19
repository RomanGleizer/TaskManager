namespace Core.Exceptions;

/// <summary>
/// Исключение, брошенное в случае ошибок валидации данных
/// </summary>
/// Инициализирует новый экземпляр класса ValidationException с заданным сообщением об ошибке и названием свойства
/// </summary>
/// <param name="message">Сообщение об ошибке</param>
/// <param name="prop">Название свойства, вызвавшего ошибку валидации</param>
public class ValidationException(string message, string prop) : Exception(message)
{
    /// <summary>
    /// Название свойства, вызвавшего ошибку валидации
    /// </summary>
    public string Property { get; init; } = prop;
}