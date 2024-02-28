using Api.Controllers.Comment.Requests;
using Api.Controllers.Comment.Responses;
using Dal.Entities;
using Logic.DTO;
using Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommentsController : ControllerBase
{
    private readonly IDtoService<CommentDTO, int> _commentService;

    public CommentsController(IDtoService<CommentDTO, int> commentService)
    {
        _commentService = commentService;
    }

    [HttpGet("{commentId}")]
    [ProducesResponseType<CommentInfoResponse>(200)]
    public async Task<IActionResult> GetInfoAsync([FromQuery] int commentId)
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

    [HttpDelete("{commentId}")]
    [ProducesResponseType<DeleteCommentResponse>(200)]
    public async Task<IActionResult> DeleteCommentAsync([FromQuery] int commentId)
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

    [HttpPut("{commentId}")]
    [ProducesResponseType<UpdateCommentResponse>(200)]
    public async Task<IActionResult> UpdateCommentAsync([FromBody] CreateCommentRequest request, [FromQuery] int commentId)
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