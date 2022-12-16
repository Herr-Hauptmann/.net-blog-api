using Microsoft.AspNetCore.Mvc;
using rubicon_blog.Dtos.Comment;
using rubicon_blog.Dtos.Post;
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
        public async Task<ActionResult<SinglePostServiceResponse<GetPostDto>>> GetSingle(string slug)
        {
            var response = await _postService.GetPostBySlug(slug);
            return (response.Success) ? Ok(response) : NotFound(response);

        }

        [HttpPost]
        public async Task<ActionResult<SinglePostServiceResponse<GetPostDto>>> AddPost(CreatePostRequest newPost){
            if (newPost == null || newPost.BlogPost == null)
                return BadRequest();
            return Ok(await _postService.AddPost(newPost.BlogPost));
        }
        [HttpPut("{slug}")]
        public async Task<ActionResult<SinglePostServiceResponse<GetPostDto>>> UpdatePost (string slug, UpdatePostRequest updatedPost){
            if (updatedPost == null || updatedPost.BlogPost == null)
                return BadRequest();
            var response = await _postService.UpdatePost(slug, updatedPost.BlogPost);
            if (response.Success == false){
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpDelete("{slug}")]
        public async Task<ActionResult<ServiceResponse<string>>> Delete (string slug){
            var response = await _postService.DeletePost(slug);
            if (response.Success == false){
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost("{slug}/comments")]
        public async Task<ActionResult<SingleCommentServiceResponse<GetCommentDto>>> AddComment(string slug, CreateCommentRequest newComment){
            if (newComment == null || newComment.Comment == null)
                return BadRequest();
            var res = await _commentService.AddComment(slug, newComment.Comment);
            return (res.Success) ? Ok(res) : BadRequest(res);
        }

        [HttpGet("{slug}/comments")]
        public async Task<ActionResult<MultipleCommentServiceResponse<List<GetCommentDto>>>> GetAllComments(string slug){
            var res = await _commentService.GetAllComments(slug);
            return (res.Success) ? Ok(res) : BadRequest(res);
        }
        [HttpDelete("{slug}/comments/{id}")]
        public async Task<ActionResult<ServiceResponse<string>>> DeleteComment(string slug, int id){
            var res = await _commentService.DeleteComment(slug, id);
            return (res.Success) ? Ok(res) : BadRequest(res);
        }
    }
}