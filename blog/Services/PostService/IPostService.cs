using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using blog.Dtos.Post;
using blog.Models.Responses;

namespace blog.Services.PostService
{
    public interface IPostService
    {
        Task<MultiplePostServiceResponse<List<GetPostDto>>> GetAllPosts(string tagName);
        Task<SinglePostServiceResponse<GetPostDto>> GetPostBySlug(string slug);
        Task<SinglePostServiceResponse<GetPostDto>> AddPost(AddPostDto newPost);
        Task<SinglePostServiceResponse<GetPostDto>> UpdatePost(string slug, UpdatePostDto updatedPost);
        Task<ServiceResponse<string>> DeletePost(string slug);
    }
}