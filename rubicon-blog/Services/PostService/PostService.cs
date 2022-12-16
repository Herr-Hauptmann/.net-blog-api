using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Slugify;
using rubicon_blog.Dtos.Post;
using Microsoft.EntityFrameworkCore;
using rubicon_blog.Services.TagService;
using rubicon_blog.Resources;
using rubicon_blog.Helpers;

namespace rubicon_blog.Services.PostService
{    
    public class PostService : IPostService
    {
        private IMapper _mapper;
        private SlugHelper _slugHelper;
        private DataContext _context;
        private ITagService _tagService;

        public PostService(ITagService tagService, IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _slugHelper = new SlugHelper();
            _context = context;
            _tagService = tagService;
        }

        public async Task<SinglePostServiceResponse<GetPostDto>> AddPost(AddPostDto newPost)
        {
            var serviceResponse = new SinglePostServiceResponse<GetPostDto>();
            try
            {
                //Create post
                Post post = Helpers.Mapper.MapAddDtoToPost(newPost);
                post.Slug = _slugHelper.GenerateSlug(newPost.Title);
                var addedPost = await _context.Posts.AddAsync(post);
                _context.SaveChanges();

                //Create tags and add them to post
                List<int> tagIds = _tagService.AddTags(newPost.TagList);
                _tagService.AddTagsToPost(addedPost.Entity.Id, tagIds);

                //Return the post
                serviceResponse.BlogPost = Helpers.Mapper.MapPostToGetDto(post);
                serviceResponse.Message = Resource.PostCreated;
                return serviceResponse;
            }
            catch(Exception ex)
            {
                serviceResponse.Exception = ex;
                serviceResponse.Success = false;
                serviceResponse.Message = Resource.PostNotCreated;
                return serviceResponse;
            }
        }

        public async Task<MultiplePostServiceResponse<List<GetPostDto>>> GetAllPosts()
        {
            var serviceResponse = new MultiplePostServiceResponse<List<GetPostDto>>();
            try
            {
                List<Post> posts = await _context.Posts.Include(t => t.Tags).ToListAsync();

                //Fill the response
                serviceResponse.BlogPosts = new List<GetPostDto>();
                foreach (var post in posts)
                    serviceResponse.BlogPosts.Add(Helpers.Mapper.MapPostToGetDto(post));
                serviceResponse.PostsCount = serviceResponse.BlogPosts.Count();
                serviceResponse.Message = Resource.PostsFetched;
                return serviceResponse;
            }
            catch(Exception ex)
            {
                serviceResponse.Exception = ex;
                serviceResponse.Success = false;
                serviceResponse.Message = Resource.PostsNotFetched;
                return serviceResponse;
            }
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
                post.UpdatedAt=DateTime.Now;
                post.Slug = _slugHelper.GenerateSlug(updatedPost.Title);
                Post updatedEntity = new();
                _mapper.Map(post, updatedEntity);
                updatedEntity.SetNullProperties(oldObj: post);
                _mapper.Map(updatedEntity, post);
                //if(updatedPost.Body == null) updatedPost.Body = post.Body; 
                //if(updatedPost.Description == null) updatedPost.Description = post.Description; 
                //if(updatedPost.Title == null) updatedPost.Title = post.Title; 
                //_mapper.Map(updatedPost, post);
                _context.SaveChanges();
                serviceResponse.Data = _mapper.Map<GetPostDto>(post);
            }
            catch(Exception)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = Resource.NoPost;
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