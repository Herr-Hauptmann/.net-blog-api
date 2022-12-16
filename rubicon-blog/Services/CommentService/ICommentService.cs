using rubicon_blog.Dtos.Comment;

namespace rubicon_blog.Services.CommentService
{
    public interface ICommentService
    {
        Task<SingleCommentServiceResponse<GetCommentDto>> AddComment(string slug, AddCommentDto newComment);
        Task<MultipleCommentServiceResponse<List<GetCommentDto>>> GetAllComments(string slug);
        Task<ServiceResponse<string>> DeleteComment(string slug, int id);
    }
}