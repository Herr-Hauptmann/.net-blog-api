using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Slugify;
using rubicon_blog.Dtos.Post;

namespace rubicon_blog.Services.PostService
{

    
    public class PostService : IPostService
    {
        private IMapper _mapper;
        private SlugHelper _slugHelper;
        private static List<Post> posts = new List<Post>();
        public PostService(IMapper mapper)
        {
            _mapper = mapper;
            _slugHelper = new SlugHelper();
        }

        public async Task<ServiceResponse<List<GetPostDto>>> AddPost(AddPostDto newPost)
        {
            var serviceResponse = new ServiceResponse<List<GetPostDto>>();
            var post = _mapper.Map<Post>(newPost);
            if (posts.FirstOrDefault() != null)
            {
                post.Id = posts.Max(post => post.Id) +1;
            }
            else post.Id = 0;
            post.Slug = _slugHelper.GenerateSlug(newPost.Title);
            posts.Add(post);
            serviceResponse.Data = posts.Select(post => _mapper.Map<GetPostDto>(post)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetPostDto>>> GetAllPosts()
        {
            return new ServiceResponse<List<GetPostDto>>{Data = posts.Select(post => _mapper.Map<GetPostDto>(post)).ToList()};
        }

        public async Task<ServiceResponse<GetPostDto>> GetPostById(string slug)
        {
            var serviceResponse = new ServiceResponse<GetPostDto>();
            var post = posts.FirstOrDefault( post=> post.Slug.Equals(slug));
            serviceResponse.Data = _mapper.Map<GetPostDto>(post);
            return serviceResponse;
        }

        public Task<ServiceResponse<GetPostDto>> UpdatePost(UpdatePostDto updatedPost)
        {
            throw new NotImplementedException();
        }
    }
}