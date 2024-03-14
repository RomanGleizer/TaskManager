using AutoMapper;
using Core.Dal.Base;
using Dal.Interfaces;
using Logic.Dto;
using Core.Exceptions;
using Dal.Entities;

namespace Logic.Services;

/// <summary>
/// Сервис для работы с пользователями. Реализует интерфейс IDtoService для объектов типа UserDTO с идентификатором типа Guid
/// </summary>
public class UserService : IDtoService<UserDTO, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    /// <summary>
    /// Конструктор класса UserService
    /// </summary>
    /// <param name="unitOfWork">Единица работы, предоставляющая доступ к операциям над данными</param>
    /// <param name="mapper">Объект для отображения объектов между различными типами, используя AutoMapper</param>
    public UserService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <inheritdoc/>
    public async Task<IList<UserDTO>> GetAllDtosAsync()
    {
        var userDals = await _unitOfWork.Users.GetAllAsync();
        return _mapper.Map<IList<UserDTO>>(userDals);
    }

    /// <inheritdoc/>
    public async Task<UserDTO?> GetDtoByIdAsync(Guid id)
    {
        var existingUserDal = await _unitOfWork.Users.GetByIdAsync(id);

        if (existingUserDal != null)
            return _mapper.Map<UserDTO>(existingUserDal);
        throw new ValidationException("User was not found in database", string.Empty);
    }

    /// <inheritdoc/>
    public async Task<UserDTO?> CreateDtoAsync(UserDTO dto)
    {
        var userDal = _mapper.Map<UserDal>(dto);
        var createdUserDal = await _unitOfWork.Users.CreateAsync(userDal);
        return _mapper.Map<UserDTO>(createdUserDal);
    }

    /// <inheritdoc/>
    public async Task<UserDTO?> DeleteDtoAsync(Guid id)
    {
        var existingUserDto = await GetDtoByIdAsync(id);
        var existingUserDal = _mapper.Map<UserDal>(existingUserDto);
        var deletedUserDal = await _unitOfWork.Users.DeleteAsync(existingUserDal);

        return _mapper.Map<UserDTO>(deletedUserDal);
    }

    /// <inheritdoc/>
    public async Task<UserDTO?> UpdateDtoAsync(UserDTO dto, Guid id)
    {
        var existingUserDal = await _unitOfWork.Users.GetByIdAsync(id);

        if (existingUserDal == null)
            throw new ValidationException("User was not found in database", string.Empty);

        var updatedUserDal = new UserDal
        {
            Id = dto.Id,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            BirthDay = dto.BirthDay,
            RoleId = dto.RoleId,
            ProjectIds = dto.ProjectIds
        };

        existingUserDal = updatedUserDal;
        updatedUserDal = await _unitOfWork.Users.UpdateAsync(existingUserDal);
        return _mapper.Map<UserDTO>(updatedUserDal);
    }
}