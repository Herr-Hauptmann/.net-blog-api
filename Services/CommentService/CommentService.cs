using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using rubicon_blog.Dtos.Comment;

namespace rubicon_blog.Services.CommentService
{
    public class CommentService : ICommentService
    {
        private IMapper _mapper;
        private DataContext _context;

        public CommentService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<ServiceResponse<GetCommentDto>> AddComment(string slug, AddCommentDto newComment)
        {
            var serviceResponse = new ServiceResponse<GetCommentDto>();
            var comment = _mapper.Map<Comment>(newComment);
            comment.CreatedAt = DateTime.Now;
            comment.UpdatedAt = DateTime.Now;
            Post post = await _context.Posts.SingleAsync(post => post.Slug.Equals(slug));
            comment.Post = post;
            await _context.Comments.AddAsync(comment);
            _context.SaveChanges();
            serviceResponse.Data = _mapper.Map<GetCommentDto>(comment);
            return serviceResponse;
        }

        public async Task<ServiceResponse<String>> DeleteComment(string slug, int id)
        {
            Comment comment = await _context.Comments.SingleAsync(c => c.Id == id);
            _context.Comments.Remove(comment);
            return new ServiceResponse<string>{Data = "Sucessfully deleted comment with id:"+id};
        }

        public async Task<ServiceResponse<List<GetCommentDto>>> GetAllComments(string slug)
        {
            try{
                int postId = (await _context.Posts.SingleAsync(post => post.Slug.Equals(slug))).Id;
                List<Comment> comments = await _context.Comments.ToListAsync();
                return new ServiceResponse<List<GetCommentDto>>{Data = comments.Select(comment => _mapper.Map<GetCommentDto>(comment)).ToList()};

            }catch (Exception)
            {
                return new ServiceResponse<List<GetCommentDto>>{Success = false, Message = "Greska!"};
            }
        }
    }
}