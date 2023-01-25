using blog.Dtos.Tag;
using blog.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace blog.Services.TagServices
{
    public interface ITagService
    {
        Task<MultipleTagServiceResponse<List<string>>> GetAllTags();
    }
}