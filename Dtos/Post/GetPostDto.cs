using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rubicon_blog.Dtos.Post
{
    public class GetPostDto
    {
        
        public int Id { get; set; } = 0;
        public string Title { get; set; } = "";
        public string Slug { get; set; } = "";
        public string Description { get; set; } = "";
        public string Body { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}