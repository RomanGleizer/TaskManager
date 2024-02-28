using Core.Dal.Base;

namespace Logic.Interfaces;

public interface IDtoService<T, I> 
    where T : IBaseDTO<I>
{
    Task<T> GetDtoByIdAsync(int id);
    IEnumerable<T> GetAllDtos();
    Task<T> CreateDtoAsync(T task);
    Task<T> DeleteDtoAsync(int id);
    Task<T> UpdateDtoAsync(T task, int id);
}
