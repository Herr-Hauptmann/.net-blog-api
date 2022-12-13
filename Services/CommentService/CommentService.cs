using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using rubicon_blog.Dtos.Comment;

namespace rubicon_blog.Services.CommentService
{
    public class CommentService : ICommentService
    {
        private static List<Comment> comments = new List<Comment>();
        private IMapper _mapper;
        public CommentService(IMapper mapper)
        {
            _mapper = mapper;
        }
        public async Task<ServiceResponse<GetCommentDto>> AddComment(string slug, AddCommentDto newComment)
        {
            var serviceResponse = new ServiceResponse<GetCommentDto>();
            var comment = _mapper.Map<Comment>(newComment);
            if (comments.FirstOrDefault() != null)
            {
                comment.Id = comments.Max(post => post.Id) +1;
            }
            else comment.Id = 0;
            comments.Add(comment);
            serviceResponse.Data = _mapper.Map<GetCommentDto>(comment);
            return serviceResponse;
        
        }

        public async Task<ServiceResponse<List<GetCommentDto>>> GetAllComments(string slug)
        {
            throw new NotImplementedException();
        }
    }
}