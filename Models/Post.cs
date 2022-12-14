using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoMapper.Configuration.Annotations;

namespace rubicon_blog.Models
{
    [Index(nameof(Slug), IsUnique = true)]
    public class Post
    {
        public int Id { get; set; } = 0;
        public string Title { get; set; } = "";
        public string Slug { get; set; } = "";
        public string Description { get; set; } = "";
        public string Body { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public virtual List<Comment> Comments { get; set; } = new List<Comment>();
        [Ignore]
        public virtual List<Tag> Tags { get; set; } = new List<Tag>();
    }
}