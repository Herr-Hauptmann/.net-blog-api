using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using rubicon_blog.Dtos.Comment;

namespace rubicon_blog.Services.CommentService
{
    public interface ICommentService
    {
        Task<ServiceResponse<GetCommentDto>> AddComment(string slug, AddCommentDto newComment);
        Task<ServiceResponse<List<GetCommentDto>>> GetAllComments(string slug);
        Task<ServiceResponse<string>> DeleteComment(string slug, int id);
    }
}