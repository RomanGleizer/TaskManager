using Logic.DTO;
using Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Api.Controllers.Comment.Requests;
using Api.Controllers.Comment.Responses;

namespace Api.Controllers;

/// <summary>
/// Контроллер для управления комментариями
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class CommentsController : ControllerBase
{
    private readonly IDtoService<CommentDTO, int> _commentService;

    /// <summary>
    /// Конструктор контроллера комментариев
    /// </summary>
    /// <param name="commentService">Сервис для работы с комментариями</param>
    public CommentsController(IDtoService<CommentDTO, int> commentService)
    {
        _commentService = commentService;
    }

    /// <summary>
    /// Получает информацию о комментарии по его идентификатору
    /// </summary>
    /// <param name="commentId">Идентификатор комментария</param>
    [HttpGet("{commentId}")]
    [ProducesResponseType<CommentInfoResponse>(200)]
    public async Task<IActionResult> GetInfoAsync([FromRoute] int commentId)
    {
        var existingComment = await _commentService.GetDtoByIdAsync(commentId);
        return Ok(new CommentInfoResponse
        {
            Id = existingComment.Id,
            Text = existingComment.Text,
            CreatedDate = existingComment.CreatedDate,
            AuthorId = existingComment.AuthorId,
            TaskId = existingComment.TaskId
        });
    }

    /// <summary>
    /// Создает новый комментарий
    /// </summary>
    /// <param name="request">Запрос на создание комментария</param>
    [HttpPost]
    [ProducesResponseType<CreateCommentResponse>(200)]
    public async Task<IActionResult> CreateCommentAsync([FromBody] CreateCommentRequest request)
    {
        var newCommentDal = await _commentService.CreateDtoAsync(new CommentDTO
        {
            Id = request.Id,
            Text = request.Text,
            CreatedDate = request.CreatedDate,
            AuthorId = request.AuthorId,
            TaskId = request.TaskId
        });

        return Ok(new CreateCommentResponse
        {
            Id = newCommentDal.Id,
            Text = newCommentDal.Text,
            CreatedDate = newCommentDal.CreatedDate,
            AuthorId = newCommentDal.AuthorId,
            TaskId = newCommentDal.TaskId
        });
    }

    /// <summary>
    /// Удаляет комментарий по его идентификатору
    /// </summary>
    /// <param name="commentId">Идентификатор комментария</param>
    [HttpDelete("{commentId}")]
    [ProducesResponseType<DeleteCommentResponse>(200)]
    public async Task<IActionResult> DeleteCommentAsync([FromRoute] int commentId)
    {
        var deletedComment = await _commentService.DeleteDtoAsync(commentId);
        return Ok(new DeleteCommentResponse
        {
            Id = deletedComment.Id,
            Text = deletedComment.Text,
            CreatedDate = deletedComment.CreatedDate,
            AuthorId = deletedComment.AuthorId,
            TaskId = deletedComment.TaskId
        });
    }

    /// <summary>
    /// Обновляет информацию о комментарии
    /// </summary>
    /// <param name="request">Запрос на обновление информации о комментарии</param>
    /// <param name="commentId">Идентификатор комментария</param>
    [HttpPut("{commentId}")]
    [ProducesResponseType<UpdateCommentResponse>(200)]
    public async Task<IActionResult> UpdateCommentAsync([FromBody] CreateCommentRequest request, [FromRoute] int commentId)
    {
        var commentDTO = new CommentDTO
        {
            Id = request.Id,
            Text = request.Text,
            CreatedDate = request.CreatedDate,
            AuthorId = request.AuthorId,
            TaskId = request.TaskId
        };

        var updatedComment = await _commentService.UpdateDtoAsync(commentDTO, commentId);
        return Ok(new UpdateCommentResponse
        {
            Id = updatedComment.Id,
            Text = updatedComment.Text,
            CreatedDate = updatedComment.CreatedDate,
            AuthorId = updatedComment.AuthorId,
            TaskId = updatedComment.TaskId
        });
    }
}
