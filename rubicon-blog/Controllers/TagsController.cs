using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using rubicon_blog.Dtos.Tag;
using rubicon_blog.Services.TagService;

namespace rubicon_blog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _tagService;
        public TagsController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet]
        public async Task<ActionResult<MultipleTagServiceResponse<List<string>>>> Get()
        {
            return Ok(await _tagService.GetAllTags());
        }
    }
}
