﻿using Core.Dal.Base;
using Dal.Entities;

namespace Dal.Interfaces;

/// <summary>
/// Представляет интерфейс для работы с единицей работы (UnitOfWork)
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Получает репозиторий для работы с ролями
    /// </summary>
    IRepository<RoleDal, int> Roles { get; }
}