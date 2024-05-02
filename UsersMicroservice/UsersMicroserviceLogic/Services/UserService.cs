using AutoMapper;
using Core.Dal.Base;
using Microsoft.AspNetCore.Identity;
using UsersMicroservice.UsersMicroserviceDal.Entities;
using UsersMicroservice.UsersMicroserviceLogic.Dto.User;
using UsersMicroservice.UsersMicroserviceLogic.Interfaces;

namespace UsersMicroservice.UsersMicroserviceLogic.Services;

/// <summary>
///     Сервис для работы с пользователями. Реализует интерфейс IDtoService для объектов типа UserDTO с идентификатором
///     типа Guid
/// </summary>
/// <remarks>
///     Конструктор класса UserService
/// </remarks>
/// <param name="userRepository">Репозиторий для пользователей</param>
/// <param name="mapper">Объект для отображения объектов между различными типами, используя AutoMapper</param>
public class UserService(IMapper mapper, IUserRepository<UserDal> userRepository) : IUserService
{
    /// <inheritdoc />
    public async Task<IList<UserDto>> GetAllUsersAsync()
    {
        var users = await userRepository.GetAllAsync();
        return mapper.Map<IList<UserDto>>(users);
    }

    /// <inheritdoc />
    public async Task<UserDto> GetUserByIdAsync(Guid id)
    {
        var existingUserDal = await userRepository.GetByIdAsync(id)
                              ?? throw new Exception("Пользователь не найден в базе данных");

        return mapper.Map<UserDto>(existingUserDal);
    }

    /// <inheritdoc />
    public async Task<IdentityResult> CreateUserAsync(CreateUserDto dto, string password)
    {
        var userDal = mapper.Map<UserDal>(dto);
        var createdEntity = await userRepository.CreateAsync(userDal, password)
                            ?? throw new Exception("Произошла ошибка при добавлении пользователя в базу данных");

        return createdEntity;
    }

    /// <inheritdoc />
    public async Task<IdentityResult> DeleteUserAsync(Guid id)
    {
        var existingUser = await userRepository.GetByIdAsync(id)
                           ?? throw new Exception("Пользователь не найден в базе данных");

        var deletedUser = await userRepository.DeleteAsync(existingUser)
                          ?? throw new Exception("Произошла ошибка при удалении пользователя из базы данных");

        return deletedUser;
    }

    /// <inheritdoc />
    public async Task<IdentityResult> UpdateUserAsync(Guid id, UpdateUserDto dto)
    {
        var existingUser = await userRepository.GetByIdAsync(id)
                           ?? throw new Exception("Пользователь не найден в базе данных");

        mapper.Map(dto, existingUser);

        var updatedUser = await userRepository.UpdateAsync(existingUser)
                          ?? throw new Exception("Произошла ошибка при обновлении пользователя в базе данных");

        return updatedUser;
    }
}