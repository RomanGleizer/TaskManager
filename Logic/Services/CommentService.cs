﻿using AutoMapper;
using Dal.Entities;
using Dal.Interfaces;
using Logic.DTO;
using Logic.Infrastructure;
using Logic.Interfaces;

namespace Logic.Services;

/// <summary>
/// Сервис для работы с комментариями
/// </summary>
public class CommentService : IDtoService<CommentDTO, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    /// <summary>
    /// Конструктор сервиса комментариев
    /// </summary>
    /// <param name="unitOfWork">Единица работы с данными</param>
    /// <param name="mapper">Маппер для преобразования между объектами</param>
    public CommentService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Получить все объекты DTO комментариев
    /// </summary>
    public IEnumerable<CommentDTO> GetAllDtos()
    {
        var allCommentDals = _unitOfWork.Comments.GetAll();
        return _mapper.Map<IEnumerable<CommentDal>, IEnumerable<CommentDTO>>(allCommentDals);
    }

    /// <summary>
    /// Получить объект DTO комментария по идентификатору асинхронно
    /// </summary>
    /// <param name="id">Идентификатор комментария</param>
    public async Task<CommentDTO> GetDtoByIdAsync(int id)
    {
        var commentDal = await _unitOfWork.Comments.GetByIdAsync(id);
        if (commentDal == null)
            throw new ValidationException("Comment was not found in database", string.Empty);
        return _mapper.Map<CommentDTO>(commentDal);
    }

    /// <summary>
    /// Создать объект DTO комментария асинхронно
    /// </summary>
    /// <param name="taskDTO">DTO комментария для создания</param>
    public async Task<CommentDTO> CreateDtoAsync(CommentDTO taskDTO)
    {
        var commentDal = _mapper.Map<CommentDal>(taskDTO);

        await _unitOfWork.Comments.CreateAsync(commentDal);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<CommentDTO>(commentDal);
    }

    /// <summary>
    /// Удалить объект DTO комментария асинхронно по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор комментария</param>
    public async Task<CommentDTO> DeleteDtoAsync(int id)
    {
        var existingCommentDal = await _unitOfWork.Comments.GetByIdAsync(id);
        if (existingCommentDal == null)
            throw new ValidationException("Comment was not found in database", string.Empty);

        _unitOfWork.Comments.Delete(existingCommentDal);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<CommentDTO>(existingCommentDal);
    }

    /// <summary>
    /// Обновить объект DTO комментария асинхронно
    /// </summary>
    /// <param name="commentDTO">DTO комментария для обновления</param>
    /// <param name="commentId">Идентификатор комментария</param>
    public async Task<CommentDTO> UpdateDtoAsync(CommentDTO commentDTO, int commentId)
    {
        var existingCommentDal = await _unitOfWork.Comments.GetByIdAsync(commentId);
        if (existingCommentDal == null)
            throw new ValidationException("Comment was not found in database", string.Empty);

        existingCommentDal.Text = commentDTO.Text;
        existingCommentDal.CreatedDate = commentDTO.CreatedDate;
        existingCommentDal.AuthorId = commentDTO.AuthorId;
        existingCommentDal.TaskId = commentDTO.TaskId;

        // Обновляется id автора и задачи, но не они сами

        _unitOfWork.Comments.Update(existingCommentDal);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<CommentDTO>(existingCommentDal);
    }
}