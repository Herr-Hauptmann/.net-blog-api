using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using rubicon_blog.Dtos.Post;
using rubicon_blog.Services.PostService;

namespace rubicon_blog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;
        public PostsController(IPostService postService){
            _postService = postService;
        }
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<GetPostDto>>>> Get()
        {
            return Ok(await _postService.GetAllPosts());
        }

        [HttpGet("{slug}")]
        public async Task<ActionResult<ServiceResponse<GetPostDto>>> GetSingle(string slug)
        {
            return Ok(await _postService.GetPostById(slug));
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<GetPostDto>>>> AddPost(AddPostDto newPost){
            return Ok(await _postService.AddPost(newPost));
        }
    }
}