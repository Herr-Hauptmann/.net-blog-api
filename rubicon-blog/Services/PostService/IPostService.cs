using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using rubicon_blog.Dtos.Post;

namespace rubicon_blog.Services.PostService
{
    public interface IPostService
    {
        Task<MultiplePostServiceResponse<List<GetPostDto>>> GetAllPosts(string tagName);
        Task<SinglePostServiceResponse<GetPostDto>> GetPostBySlug(string slug);
        Task<SinglePostServiceResponse<GetPostDto>> AddPost(AddPostDto newPost);
        Task<ServiceResponse<GetPostDto>> UpdatePost(string slug, UpdatePostDto updatedPost);
        Task<ServiceResponse<List <GetPostDto>>> DeletePost(string slug);
    }
}