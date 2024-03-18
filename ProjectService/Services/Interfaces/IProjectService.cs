using Services.ViewModels.ProjectViewModels;

namespace Services.Interfaces;

/// <summary>
/// Сервис для работы с проектами
/// </summary>
public interface IProjectService
{
    /// <summary>
    /// Получает информацию о проекте по его идентификатору
    /// </summary>
    /// <param name="id">Идентификатор проекта</param>
    /// <returns>Модель представления проекта или null, если проект не найден</returns>
    Task<ProjectViewModel> GetById(int id);

    /// <summary>
    /// Создает новый проект
    /// </summary>
    /// <param name="model">Модель создания проекта</param>
    /// <returns>Модель представления созданного проекта или null, если операция не удалась</returns>
    Task<ProjectViewModel> Create(CreateProjectViewModel model);

    /// <summary>
    /// Удаляет проект по его идентификатору
    /// </summary>
    /// <param name="id">Идентификатор проекта для удаления</param>
    /// <returns>Модель представления удаленного проекта или null, если операция не удалась</returns>
    Task<ProjectViewModel> Delete(int id);

    /// <summary>
    /// Обновляет информацию о проекте
    /// </summary>
    /// <param name="id">Идентификатор проекта для обновления</param>
    /// <param name="model">Модель обновления проекта</param>
    /// <returns>Модель представления обновленного проекта или null, если операция не удалась</returns>
    Task<ProjectViewModel> Update(int id, UpdateProjectViewModel model);
}