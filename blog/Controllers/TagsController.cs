using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using blog.Dtos.Tag;
using blog.Models.Responses;
using blog.Services.TagServices;

namespace blog.Controllers
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
