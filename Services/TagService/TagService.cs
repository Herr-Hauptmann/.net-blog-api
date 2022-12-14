using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace rubicon_blog.Services.TagService
{
    public class TagService : ITagService
    {
        private IMapper _mapper;
        private DataContext _context;

        public TagService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public List<int> AddTags(List<string>? tagNames){
            List<int> tagIds = new List<int>();
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
    }
}