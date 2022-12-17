using rubicon_blog.Dtos.Tag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rubicon_blog.Services.TagService
{
    public interface ITagService
    {
        Task<MultipleTagServiceResponse<List<string>>> GetAllTags();
    }
}