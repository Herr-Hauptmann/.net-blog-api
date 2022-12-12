using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using rubicon_blog.Dtos.Post;

namespace rubicon_blog.Services.PostService
{
    public interface IPostService
    {
        Task<ServiceResponse<List<GetPostDto>>> GetAllPosts();
        Task<ServiceResponse<GetPostDto>> GetPostById(string slug);
        Task<ServiceResponse<List <GetPostDto>>> AddPost(AddPostDto newPost);
        Task<ServiceResponse<GetPostDto>> UpdatePost(string slug, UpdatePostDto updatedPost);
        Task<ServiceResponse<List <GetPostDto>>> DeletePost(string slug);
    }
}