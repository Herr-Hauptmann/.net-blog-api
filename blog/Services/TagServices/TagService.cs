using AutoMapper;
using Microsoft.EntityFrameworkCore;
using blog.Resources;
using blog.Models.Responses;

namespace blog.Services.TagServices
{
    public class TagService : ITagService
    {
        private readonly DataContext _context;

        public TagService(IMapper mapper, DataContext context)
        {
            _context = context;
        }


        public async Task<MultipleTagServiceResponse<List<string>>> GetAllTags()
        {
            var serviceResponse = new MultipleTagServiceResponse<List<string>>();
            try
            {
                var tags = await _context.Tags.ToListAsync();
                serviceResponse.Tags = tags.Select(tag => tag.Name).ToList();
                serviceResponse.Message = Resource.TagsFetched;
            }
            catch (Exception ex)
            {
                serviceResponse.Exception = ex;
                serviceResponse.Success = false;
                serviceResponse.Message = Resource.TagsNotFetched;
            }
            return serviceResponse;
        }


    }
}