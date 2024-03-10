using AutoMapper;
using Core.Exceptions;
using Dal.Entities;
using Logic.Dto.User;
using Logic.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Logic.Services;

/// <summary>
/// Сервис для работы с пользователями. Реализует интерфейс IDtoService для объектов типа UserDTO с идентификатором типа Guid
/// </summary>
/// <remarks>
/// Конструктор класса UserService
/// </remarks>
/// <param name="unitOfWork">Единица работы, предоставляющая доступ к операциям над данными</param>
/// <param name="mapper">Объект для отображения объектов между различными типами, используя AutoMapper</param>
public class UserService(IMapper mapper, UserManager<UserDal> userManager)
    : IUserService<UserDto, Guid>
{
    private readonly IMapper _mapper = mapper;
    private readonly UserManager<UserDal> _userManager = userManager;

    /// <inheritdoc/>
    public async Task<IList<UserDto>> GetAllUsersAsync()
    {
        var userDals = await _userManager.Users.ToListAsync();
        return _mapper.Map<IList<UserDto>>(userDals);
    }

    /// <inheritdoc/>
    public async Task<UserDto> GetUserByIdAsync(Guid id)
    {
        var existingUserDal = await _userManager.FindByIdAsync(id.ToString());

        return existingUserDal != null
            ? _mapper.Map<UserDto>(existingUserDal)
            : throw new ValidationException("User was not found in database", string.Empty);
    }

    /// <inheritdoc/>
    public async Task<IdentityResult> CreateUserAsync(UserDto dto)
    {
        var userDal = _mapper.Map<UserDal>(dto);
        return await _userManager.CreateAsync(userDal, dto.Password);
    }

    /// <inheritdoc/>
    public async Task<IdentityResult> DeleteUserAsync(Guid id)
    {
        var existingUserDto = await GetUserByIdAsync(id);
        var existingUserDal = _mapper.Map<UserDal>(existingUserDto);

        return await _userManager.DeleteAsync(existingUserDal);
    }

    /// <inheritdoc/>
    public async Task<IdentityResult> UpdateUserAsync(UserDto dto, Guid id)
    {
        var existingUserDal = await _userManager.FindByIdAsync(id.ToString()) 
            ?? throw new ValidationException("User was not found in database", string.Empty);

        _mapper.Map(dto, existingUserDal);
        return await _userManager.UpdateAsync(existingUserDal);
    }

    /// <inheritdoc/>
    public async Task<IdentityResult> AddNewProject(int projectId, Guid memberId)
    {
        var existingUserDal = await _userManager.FindByIdAsync(memberId.ToString()) 
            ?? throw new ValidationException("User was not found in database", string.Empty);

        existingUserDal.ProjectIds.Add(projectId);
        return await _userManager.UpdateAsync(existingUserDal);
    }
}