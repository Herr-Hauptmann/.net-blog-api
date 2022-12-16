using rubicon_blog.Dtos.Post;

namespace rubicon_blog.Wrappers
{
    public class CreatePostRequest
    {
        public AddPostDto? BlogPost { get; set; }
    }
}
