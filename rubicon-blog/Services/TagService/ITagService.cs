using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rubicon_blog.Services.TagService
{
    public interface ITagService
    {
        public List<int> AddTags(List<string>? tagNames);
        void AddTagsToPost(int postId, List<int> tagIds);
        public List<Post> GetPostsByTag(string tagName);
        public void deleteTags(List<Tag> tags);
    }
}