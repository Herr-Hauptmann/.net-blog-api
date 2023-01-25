using blog.Dtos.Comment;
using blog.Models.Responses;

namespace blog.Services.CommentService
{
    public interface ICommentService
    {
        Task<SingleCommentServiceResponse<GetCommentDto>> AddComment(string slug, AddCommentDto newComment);
        Task<MultipleCommentServiceResponse<List<GetCommentDto>>> GetAllComments(string slug);
        Task<ServiceResponse<string>> DeleteComment(string slug, int id);
    }
}