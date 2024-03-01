/// <summary>
/// Статус выполнения задачи
/// </summary>
public enum ExecutionStatus
{
    /// <summary>
    /// Ожидание выполнения
    /// </summary>
    Pending = 1,

    /// <summary>
    /// В процессе выполнения
    /// </summary>
    Implementation = 2,

    /// <summary>
    /// Тестирование
    /// </summary>
    Testing = 3,

    /// <summary>
    /// Завершено
    /// </summary>
    Completed = 4,
}
