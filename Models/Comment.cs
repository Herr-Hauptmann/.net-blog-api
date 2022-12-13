using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rubicon_blog.Models
{
    public class Comment
    {
        public int Id { get; set; } = 0;
        public int postId{get;set;} = 0;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Body { get; set; } = "";
        public Post Post { get; set; }
    }
}