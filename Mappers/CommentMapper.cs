using api.Dtos.Comment;
using api.Models;

namespace api.Mappers;

public static class CommentMapper
{
  public static CommentDto ToCommentDto(this Comment commentModel) => new()
  {
    Id = commentModel.Id,
    Title = commentModel.Title,
    Content = commentModel.Content,
    CreatedOn = commentModel.CreatedOn,
    CreatedBy = commentModel.AppUser.UserName,
    StockId = commentModel.StockId
  };
  public static Comment ToCommentFromCreate(this CreateCommentDto commentDto, int stockId) => new()
  {
    Title = commentDto.Title,
    Content = commentDto.Content,
    StockId = stockId,
  };
  public static Comment ToCommentFromUpdate(this UpdateCommentRequestDto commentDto) => new()
  {
    Title = commentDto.Title,
    Content = commentDto.Content
  };
}
