using AutoMapper;
using Dal.Entities;
using Dal.Interfaces;
using Logic.DTO;
using Core.Dal.Base;
using Core.Exceptions;

namespace Logic.Services;

/// <summary>
/// Сервис для работы с комментариями
/// </summary>
/// <remarks>
/// Конструктор сервиса комментариев
/// </remarks>
/// <param name="unitOfWork">Единица работы с данными</param>
/// <param name="mapper">Маппер для преобразования между объектами</param>
public class CommentService(IUnitOfWork unitOfWork, IMapper mapper) : IDtoService<CommentDTO, int>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    /// <inheritdoc/>
    public async Task<IList<CommentDTO>> GetAllDtosAsync()
    {
        var allCommentDals = await _unitOfWork.Comments.GetAllAsync();
        return _mapper.Map<IList<CommentDal>, IList<CommentDTO>>(allCommentDals);
    }

    /// <inheritdoc/>
    public async Task<CommentDTO> GetDtoByIdAsync(int id)
    {
        var commentDal = await _unitOfWork.Comments.GetByIdAsync(id);
        return commentDal == null
            ? throw new ValidationException("Comment was not found in database", string.Empty)
            : _mapper.Map<CommentDTO>(commentDal);
    }

    /// <inheritdoc/>
    public async Task<CommentDTO> CreateDtoAsync(CommentDTO taskDTO)
    {
        var commentDal = _mapper.Map<CommentDal>(taskDTO);

        await _unitOfWork.Comments.CreateAsync(commentDal);
        return _mapper.Map<CommentDTO>(commentDal);
    }

    /// <inheritdoc/>
    public async Task<CommentDTO> DeleteDtoAsync(int id)
    {
        var existingCommentDal = await _unitOfWork.Comments.GetByIdAsync(id) ?? throw new ValidationException("Comment was not found in database", string.Empty);
        await _unitOfWork.Comments.DeleteAsync(existingCommentDal);
        return _mapper.Map<CommentDTO>(existingCommentDal);
    }

    /// <inheritdoc/>
    public async Task<CommentDTO> UpdateDtoAsync(CommentDTO commentDTO, int commentId)
    {
        var existingCommentDal = await _unitOfWork.Comments.GetByIdAsync(commentId) ?? throw new ValidationException("Comment was not found in database", string.Empty);
        existingCommentDal = existingCommentDal with
        {
            Text = commentDTO.Text,
            CreatedDate = commentDTO.CreatedDate,
            AuthorId = commentDTO.AuthorId,
            TaskId = commentDTO.TaskId,
        };

        await _unitOfWork.Comments.UpdateAsync(existingCommentDal);
        return _mapper.Map<CommentDTO>(existingCommentDal);
    }
}
