using AutoMapper;
using Microsoft.EntityFrameworkCore;
using rubicon_blog.Dtos.Comment;
using rubicon_blog.Resources;

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
        public async Task<SingleCommentServiceResponse<GetCommentDto>> AddComment(string slug, AddCommentDto newComment)
        {
            var serviceResponse = new SingleCommentServiceResponse<GetCommentDto>();
            try
            {
                var comment = _mapper.Map<Comment>(newComment);
                comment.CreatedAt = DateTime.Now;
                comment.UpdatedAt = DateTime.Now;
                Post post = await _context.Posts.SingleAsync(post => post.Slug.Equals(slug));
                comment.Post = post;
                await _context.Comments.AddAsync(comment);
                _context.SaveChanges();
                serviceResponse.Comment = _mapper.Map<GetCommentDto>(comment);
                serviceResponse.Message = Resource.CommentCreated;
            }
            catch(Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Exception = ex;
                serviceResponse.Message = Resource.CommentNotCreated;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<string>> DeleteComment(string slug, int id)
        {
            var serviceResponse = new ServiceResponse<string>();
            try
            {
                Comment? comment = await _context.Comments.SingleOrDefaultAsync(c => c.Id == id);
                if (comment == null)
                {
                    serviceResponse.Message = Resource.CommentNotFound;
                    serviceResponse.Success = false;
                    return serviceResponse;
                }
                _context.Comments.Remove(comment);
                _context.SaveChanges();
                serviceResponse.Message = Resource.CommentDeleted;

            }
            catch(Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Exception = ex;
                serviceResponse.Message = Resource.CommentNotDeleted;
            }
            return serviceResponse;
        }

        public async Task<MultipleCommentServiceResponse<List<GetCommentDto>>> GetAllComments(string slug)
        {
            var serviceResponse = new MultipleCommentServiceResponse<List<GetCommentDto>>();
            try{
                var post = await _context.Posts.Include(p => p.Comments).SingleAsync(post => post.Slug.Equals(slug));
                serviceResponse.Comments = post.Comments.Select(comment => _mapper.Map<GetCommentDto>(comment)).ToList();
                serviceResponse.Message = Resource.CommentsFetched;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Exception = ex;
                serviceResponse.Message = Resource.CommentsNotFetched;
            }
            return serviceResponse;
        }
    }
}