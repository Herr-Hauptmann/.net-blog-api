using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rubicon_blog.Dtos.Post
{
    public class AddPostDto
    {
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Body { get; set; } = "";
    }
}