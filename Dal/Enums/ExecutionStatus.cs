/// <summary>
/// Статус выполнения задачи
/// </summary>
public enum ExecutionStatus
{
    /// <summary>
    /// Ожидание выполнения
    /// </summary>
    Pending,

    /// <summary>
    /// В процессе выполнения
    /// </summary>
    Implementation,

    /// <summary>
    /// Тестирование
    /// </summary>
    Testing,

    /// <summary>
    /// Завершено
    /// </summary>
    Completed,
}
