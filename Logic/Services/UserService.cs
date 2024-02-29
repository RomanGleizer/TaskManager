using AutoMapper;
using Dal.Entities;
using Dal.Interfaces;
using Logic.DTO;
using Logic.Infrastructure;
using Logic.Interfaces;

namespace Logic.Services;

/// <summary>
/// Сервис для работы с пользователями
/// </summary>
public class UserService : IDtoService<UserDTO, string>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    /// <summary>
    /// Инициализирует новый экземпляр класса UserService
    /// </summary>
    /// <param name="unitOfWork">Объект единицы работы</param>
    /// <param name="mapper">Объект маппера</param>
    public UserService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Получает все DTO пользователей
    /// </summary>
    public IEnumerable<UserDTO> GetAllDtos()
    {
        var allUserDals = _unitOfWork.Users.GetAll();
        return _mapper.Map<IEnumerable<UserDal>, IEnumerable<UserDTO>>(allUserDals);
    }

    /// <summary>
    /// Асинхронно получает DTO пользователя по его идентификатору
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    public async Task<UserDTO> GetDtoByIdAsync(string userId)
    {
        var userDal = await GetExistingUserAsync(userId);
        return _mapper.Map<UserDTO>(userDal);
    }

    /// <summary>
    /// Асинхронно создает DTO пользователя
    /// </summary>
    /// <param name="userDTO">DTO пользователя для создания</param>
    public async Task<UserDTO> CreateDtoAsync(UserDTO userDTO)
    {
        var userDal = _mapper.Map<UserDal>(userDTO);

        await _unitOfWork.Users.CreateAsync(userDal);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<UserDTO>(userDal);
    }

    /// <summary>
    /// Асинхронно удаляет DTO пользователя по его идентификатору
    /// </summary>
    /// <param name="userId">Идентификатор пользователя для удаления</param>
    public async Task<UserDTO> DeleteDtoAsync(string userId)
    {
        var existingUserDal = await GetExistingUserAsync(userId);

        _unitOfWork.Users.Delete(existingUserDal);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<UserDTO>(existingUserDal);
    }

    /// <summary>
    /// Асинхронно обновляет DTO пользователя по его идентификатору
    /// </summary>
    /// <param name="userDTO">Обновленные данные DTO пользователя</param>
    /// <param name="userId">Идентификатор пользователя для обновления</param>
    public async Task<UserDTO> UpdateDtoAsync(UserDTO userDTO, string userId)
    {
        var existingUserDal = await GetExistingUserAsync(userId);

        existingUserDal.FirstName = userDTO.FirstName;
        existingUserDal.LastName = userDTO.LastName;
        existingUserDal.Email = userDTO.Email;
        existingUserDal.PhoneNumber = userDTO.PhoneNumber;
        existingUserDal.BirthDay = userDTO.BirthDay;
        existingUserDal.RoleId = userDTO.RoleId;

        var role = await _unitOfWork.Roles.GetByIdAsync(userDTO.RoleId);
        if (role != null)
            existingUserDal.Role = role;

        _unitOfWork.Users.Update(existingUserDal);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<UserDTO>(existingUserDal);
    }

    /// <summary>
    /// Асинхронно получает существующего пользователя по его идентификатору
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    private async Task<UserDal> GetExistingUserAsync(string userId)
    {
        var existingUserDal = await _unitOfWork.Users.GetByIdAsync(userId);
        if (existingUserDal == null)
            throw new ValidationException("Пользователь не найден в базе данных", string.Empty);
        return existingUserDal;
    }
}
