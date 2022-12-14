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
    }
}