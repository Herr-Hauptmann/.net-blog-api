using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using rubicon_blog.Dtos.Comment;
using rubicon_blog.Dtos.Post;
using rubicon_blog.Helpers;
using rubicon_blog.Services.CommentService;
using rubicon_blog.Services.PostService;
using rubicon_blog.Wrappers;

namespace rubicon_blog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly ICommentService _commentService;
        public PostsController(IPostService postService, ICommentService commentService){
            _postService = postService;
            _commentService = commentService;
        }

        [HttpGet]
        public async Task<ActionResult<MultiplePostServiceResponse<List<GetPostDto>>>> Get()
        {
            string tag = HttpContext.Request.Query["tag"].ToString();
            return Ok(await _postService.GetAllPosts(tag));
        }

        [HttpGet("{slug}")]
        public async Task<ActionResult<ServiceResponse<GetPostDto>>> GetSingle(string slug)
        {
            return Ok(await _postService.GetPostBySlug(slug));
        }

        [HttpPost]
        public async Task<ActionResult<SinglePostServiceResponse<GetPostDto>>> AddPost(CreatePostRequest newPost){
            if (newPost == null || newPost.BlogPost == null)
                return BadRequest();
            return Ok(await _postService.AddPost(newPost.BlogPost));
        }
        [HttpPut("{slug}")]
        public async Task<ActionResult<ServiceResponse<GetPostDto>>> UpdatePost (string slug, UpdatePostDto updatedPost){
            var response = await _postService.UpdatePost(slug, updatedPost);
            if (response.Data == null){
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpDelete("{slug}")]
        public async Task<ActionResult<ServiceResponse<List<GetPostDto>>>> Delete (string slug){
            var response = await _postService.DeletePost(slug);
            if (response.Data == null){
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost("{slug}/comments")]
        public async Task<ActionResult<ServiceResponse<GetCommentDto>>> AddComment(string slug, AddCommentDto newComment){
            return Ok(await _commentService.AddComment(slug, newComment));
        }

        [HttpGet("{slug}/comments")]
        public async Task<ActionResult<ServiceResponse<List<GetCommentDto>>>> GetAllComments(string slug){
            return Ok(await _commentService.GetAllComments(slug));
        }
        [HttpDelete("{slug}/comments/{id}")]
        public async Task<ActionResult<ServiceResponse<string>>> DeleteComment(string slug, int id){
            return Ok(await _commentService.DeleteComment(slug, id));
        }
    }
}