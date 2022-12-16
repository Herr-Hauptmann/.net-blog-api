using rubicon_blog.Dtos.Post;
using rubicon_blog.Dtos.Tag;

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

        public static void MapUpdatePost(Post post, UpdatePostDto updatedPost)
        {
            if (updatedPost.Body != null)
                post.Body = updatedPost.Body;
            if (updatedPost.Description !=null)
                post.Description = updatedPost.Description;
            if (updatedPost.Title != null)
                post.Title = updatedPost.Title;
        }

        public static GetTagDto TagToGetDto(Tag t)
        {
            return new GetTagDto
            {
                Name = t.Name
            };
        }
    }
}
