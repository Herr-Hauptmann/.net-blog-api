using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace rubicon_blog.Services.TagService
{
    public class TagService : ITagService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public TagService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public List<int> AddTags(List<string>? tagNames){
            List<int> tagIds = new();
            if (tagNames ==null)
                return tagIds;
            
            foreach(string tagName in tagNames)
            {
                var t = _context.Tags.SingleOrDefault(t => t.Name.Equals(tagName));
                if (t == null)
                {
                    _context.Tags.Add(new Tag{Name=tagName});
                    _context.SaveChanges();
                    t = _context.Tags.SingleOrDefault(t => t.Name.Equals(tagName));
                }
                if (t!=null)
                    tagIds.Add(t.Id);
            }
            
            return tagIds;
        }

        public void AddTagsToPost(int postId, List<int> tagIds)
        {
            var post = _context.Posts.Find(postId);
            if (post == null)
                throw new Exception("Post ne postoji!");
            
            foreach(int id in tagIds){
                var tag = _context.Tags.FirstOrDefault(t => t.Id == id);
                if (tag != null)
                {
                    post.Tags.Add(tag);
                }
            }
            _context.SaveChanges();
        }

        public List<Post> GetPostsByTag(string tagName)
        {
            List<Post> posts = new List<Post>();
            try
            {
                //very crude but it only returns one tag
                var tag = _context.Tags.Include(p => p.Posts).SingleOrDefault(tag => tag.Name.Equals(tagName));
                if (tag != null)
                {
                    foreach (Post p in tag.Posts)
                    {
                        var post = _context.Posts.Include(t => t.Tags).Single(post => post.Id == p.Id);
                        posts.Add(post);
                    }
                }
                posts = posts.OrderByDescending(p => p.CreatedAt).ToList();
                return posts;
            }catch(Exception )
            {
                return posts;
            }
        }
    }
}