using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Configuration.Annotations;

namespace blog.Dtos.Post
{
    public class AddPostDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        [Ignore]
        public List<String>? TagList {get; set;}
    }
}