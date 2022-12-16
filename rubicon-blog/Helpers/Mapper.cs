using rubicon_blog.Dtos.Post;

namespace rubicon_blog.Helpers
{
    public static class Mapper
    {
        public static GetPostDto MapPostToGetDto(Post post)
        {
            return new GetPostDto()
            {
                Slug = post.Slug,
                Title = post.Title,
                Description = post.Description,
                Body = post.Body,
                tagList = post.Tags.Select(tag => tag.Name).ToList(),
                CreatedAt = post.CreatedAt,
                UpdatedAt = post.UpdatedAt
            };
        }

        public static Post MapAddDtoToPost(AddPostDto newPost)
        {
            return new Post()
            {
                Title = newPost.Title,
                Body = newPost.Body,
                Description = newPost.Description,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
        }
    }
}
