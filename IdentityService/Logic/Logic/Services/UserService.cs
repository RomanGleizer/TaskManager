using AutoMapper;
using Core.Exceptions;
using Dal.Entities;
using Dal.Interfaces;
using Logic.Dto.User;
using Logic.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Logic.Services;

/// <summary>
/// Сервис для работы с пользователями. Реализует интерфейс IDtoService для объектов типа UserDTO с идентификатором типа Guid
/// </summary>
/// <remarks>
/// Конструктор класса UserService
/// </remarks>
/// <param name="userRepository">Репозиторий для пользователей</param>
/// <param name="mapper">Объект для отображения объектов между различными типами, используя AutoMapper</param>
public class UserService(IMapper mapper, IUserRepository userRepository) : IUserService
{
    private readonly IMapper _mapper = mapper;
    private readonly IUserRepository _userRepository = userRepository;

    /// <inheritdoc/>
    public async Task<IList<UserDto>> GetAllUsersAsync()
    {
        var userDals = await _userRepository.GetAllAsync();
        return _mapper.Map<IList<UserDto>>(userDals);
    }

    /// <inheritdoc/>
    public async Task<UserDto> GetUserByIdAsync(Guid id)
    {
        var existingUserDal = await _userRepository.GetByIdAsync(id)
            ?? throw new ValidationException("Пользователь не найден в базе данных", string.Empty);

        return _mapper.Map<UserDto>(existingUserDal);
    }

    /// <inheritdoc/>
    public async Task<IdentityResult> CreateUserAsync(CreateUserDto dto, string password)
    {
        var userDal = _mapper.Map<UserDal>(dto);
        var createdEntity = await _userRepository.CreateAsync(userDal, password)
            ?? throw new Exception("Произошла ошибка при добавлении пользователя в базу данных");

        return createdEntity;
    }

    /// <inheritdoc/>
    public async Task<IdentityResult> DeleteUserAsync(Guid id)
    {
        var existingUser = await _userRepository.GetByIdAsync(id)
            ?? throw new Exception("Пользователь не найден в базе данных");

        var deletedUser = await _userRepository.DeleteAsync(existingUser)
            ?? throw new Exception("Произошла ошибка при удалении пользователя из базы данных");

        return deletedUser;
    }

    /// <inheritdoc/>
    public async Task<IdentityResult> UpdateUserAsync(Guid id, UpdateUserDto dto)
    {
        var existingUser = await _userRepository.GetByIdAsync(id)
            ?? throw new Exception("Пользователь не найден в базе данных");

        _mapper.Map(dto, existingUser);

        var updatedUser = await _userRepository.UpdateAsync(existingUser)
            ?? throw new Exception("Произошла ошибка при обновлении пользователя в базе данных");

        return updatedUser;
    }

    /// <inheritdoc/>
    public async Task<IdentityResult> AddNewProject(int projectId, Guid memberId)
    {
        var existingUserDal = await _userRepository.GetByIdAsync(memberId)
            ?? throw new ValidationException("Пользователь не найден в базе данных", string.Empty);

        existingUserDal.ProjectIds.Add(projectId);
        var updatedUser = await _userRepository.UpdateAsync(existingUserDal)
            ?? throw new Exception("Произошла ошибка при обновлении проектов пользователя в базе данных");

        return updatedUser;
    }
}