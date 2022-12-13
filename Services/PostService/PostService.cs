using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Slugify;
using rubicon_blog.Dtos.Post;
using Microsoft.EntityFrameworkCore;

namespace rubicon_blog.Services.PostService
{

    
    public class PostService : IPostService
    {
        private IMapper _mapper;
        private SlugHelper _slugHelper;
        private DataContext _context;
        public PostService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _slugHelper = new SlugHelper();
            _context = context;
        }

        public async Task<ServiceResponse<GetPostDto>> AddPost(AddPostDto newPost)
        {
            var serviceResponse = new ServiceResponse<GetPostDto>();
            Post post = _mapper.Map<Post>(newPost);
            post.Slug = _slugHelper.GenerateSlug(newPost.Title);
            await _context.Posts.AddAsync(post);
            _context.SaveChanges();
            serviceResponse.Data = _mapper.Map<GetPostDto>(post);
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetPostDto>>> GetAllPosts()
        {
            List<Post> posts = await _context.Posts.ToListAsync();
            return new ServiceResponse<List<GetPostDto>>{Data = posts.Select(post => _mapper.Map<GetPostDto>(post)).ToList()};
        }

        public async Task<ServiceResponse<GetPostDto>> GetPostBySlug(string slug)
        {
            var serviceResponse = new ServiceResponse<GetPostDto>();
            Post post = await _context.Posts.SingleAsync(post => post.Slug.Equals(slug));
            serviceResponse.Data = _mapper.Map<GetPostDto>(post);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetPostDto>> UpdatePost(string slug, UpdatePostDto updatedPost)
        {
            var serviceResponse = new ServiceResponse<GetPostDto>();
            try
            { 
                Post post = await _context.Posts.SingleAsync(post => post.Slug.Equals(slug));
                _mapper.Map(updatedPost, post);
                _context.SaveChanges();
                serviceResponse.Data = _mapper.Map<GetPostDto>(post);
            }
            catch(Exception)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "This post doesn't exist";
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetPostDto>>> DeletePost(string slug)
        {
            var serviceResponse = new ServiceResponse<List<GetPostDto>>();
            try{
                Post post = await _context.Posts.SingleAsync(post => post.Slug.Equals(slug));
                _context.Posts.Remove(post);
                _context.SaveChanges();
                serviceResponse.Message = "Post deleted successfully";
            }catch(Exception)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "This post doesn't exist";
            }
            return serviceResponse;
        }
    }
}