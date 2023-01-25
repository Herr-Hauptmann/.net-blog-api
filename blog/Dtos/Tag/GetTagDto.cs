using blog.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace blog.Dtos.Tag
{
    public class GetTagDto
    {
        public string Name { get; set; } = "";

        public Task<ServiceResponse<List<GetTagDto>>> GetAllTags()
        {
            throw new NotImplementedException();
        }
    }
}