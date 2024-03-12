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
            ?? throw new ValidationException("User was not found in database", string.Empty);

        return _mapper.Map<UserDto>(existingUserDal);
    }

    /// <inheritdoc/>
    public async Task<IdentityResult> CreateUserAsync(CreateUserDto dto, string password)
    {
        var userDal = _mapper.Map<UserDal>(dto);
        var createdEntity = await _userRepository.CreateAsync(userDal, password)
            ?? throw new Exception("An error occurred while creating the user to the database");

        return createdEntity;
    }

    /// <inheritdoc/>
    public async Task<IdentityResult> DeleteUserAsync(Guid id)
    {
        var existingUser = await _userRepository.GetByIdAsync(id)
                    ?? throw new Exception("User was not found in database");

        var deletedUser = await _userRepository.DeleteAsync(existingUser)
            ?? throw new Exception("An error occurred while deleting the user to the database");

        return deletedUser;
    }

    /// <inheritdoc/>
    public async Task<IdentityResult> UpdateUserAsync(Guid id, UpdateUserDto dto)
    {
        var existingUser = await _userRepository.GetByIdAsync(id)
            ?? throw new Exception("User was not found in database");

        _mapper.Map(dto, existingUser);

        var updatedUser = await _userRepository.UpdateAsync(existingUser)
            ?? throw new Exception("An error occurred while updating the user to the database");

        return updatedUser;
    }

    /// <inheritdoc/>
    public async Task<IdentityResult> AddNewProject(int projectId, Guid memberId)
    {
        var existingUserDal = await _userRepository.GetByIdAsync(memberId)
            ?? throw new ValidationException("User was not found in database", string.Empty);

        existingUserDal.ProjectIds.Add(projectId);
        var updatedUser = await _userRepository.UpdateAsync(existingUserDal)
            ?? throw new Exception("An error occurred while updating the user's projects to the database");

        return updatedUser;
    }
}