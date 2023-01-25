using Slugify;
using blog.Dtos.Post;
using Microsoft.EntityFrameworkCore;
using blog.Services.TagServices;
using blog.Resources;
using blog.Models.Responses;

namespace blog.Services.PostService
{
    public class PostService : IPostService
    {
        private readonly DataContext _context;

        public PostService(DataContext context)
        {
            _context = context;
        }

        public async Task<SinglePostServiceResponse<GetPostDto>> AddPost(AddPostDto newPost)
        {
            var serviceResponse = new SinglePostServiceResponse<GetPostDto>();
            try
            {
                SlugHelper _slugHelper = new SlugHelper();
                //Create post
                Post post = Helpers.Mapper.MapAddDtoToPost(newPost);
                post.Slug = _slugHelper.GenerateSlug(newPost.Title);
                var addedPost = await _context.Posts.AddAsync(post);
                _context.SaveChanges();

                //Create tags and add them to post
                List<int> tagIds = AddTags(newPost.TagList);
                AddTagsToPost(addedPost.Entity.Id, tagIds);

                //Return the post
                serviceResponse.BlogPost = Helpers.Mapper.MapPostToGetDto(post);
                serviceResponse.Message = Resource.PostCreated;
                return serviceResponse;
            }
            catch(Exception ex)
            {
                serviceResponse.Exception = ex;
                serviceResponse.Success = false;
                serviceResponse.Message = Resource.PostNotCreated;
                return serviceResponse;
            }
        }

        public async Task<MultiplePostServiceResponse<List<GetPostDto>>> GetAllPosts(string tagName)
        {
            var serviceResponse = new MultiplePostServiceResponse<List<GetPostDto>>();
            try
            {
                List<Post> posts;
                if (tagName != null && tagName.Length != 0)
                    posts = GetPostsByTag(tagName);
                else
                    posts = await _context.Posts.Include(t => t.Tags).OrderByDescending(p=>p.CreatedAt).ToListAsync();

                //Fill the response
                serviceResponse.BlogPosts = new List<GetPostDto>();
                foreach (var post in posts)
                    serviceResponse.BlogPosts.Add(Helpers.Mapper.MapPostToGetDto(post));
                serviceResponse.PostsCount = serviceResponse.BlogPosts.Count();
                serviceResponse.Message = Resource.PostsFetched;
                return serviceResponse;
            }
            catch(Exception ex)
            {
                serviceResponse.Exception = ex;
                serviceResponse.Success = false;
                serviceResponse.Message = Resource.PostsNotFetched;
                return serviceResponse;
            }
        }

        public async Task<SinglePostServiceResponse<GetPostDto>> GetPostBySlug(string slug)
        {
            var serviceResponse = new SinglePostServiceResponse<GetPostDto>();
            try
            {
                Post? post = await _context.Posts.Include(t => t.Tags).SingleOrDefaultAsync(post => post.Slug.Equals(slug));
                if (post == null)
                    throw new Exception("Not found");
                serviceResponse.BlogPost = Helpers.Mapper.MapPostToGetDto(post);
                serviceResponse.Message = Resource.PostsFetched;
            }
            catch(Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = Resource.PostNotFound;
                serviceResponse.Exception = ex;
            }
            return serviceResponse;
        }

        public async Task<SinglePostServiceResponse<GetPostDto>> UpdatePost(string slug, UpdatePostDto updatedPost)
        {
            SlugHelper _slugHelper = new SlugHelper();
            var serviceResponse = new SinglePostServiceResponse<GetPostDto>();
            try
            { 
                Post post = await _context.Posts.Include(p => p.Tags).SingleAsync(post => post.Slug.Equals(slug));
                Helpers.Mapper.MapUpdatePost(post, updatedPost);
                post.Slug = _slugHelper.GenerateSlug(post.Title);
                post.UpdatedAt = DateTime.Now;
                _context.SaveChanges();
                serviceResponse.BlogPost = Helpers.Mapper.MapPostToGetDto(post);
                serviceResponse.Message = Resource.PostUpdated;
            }
            catch(Exception ex)
            {
                serviceResponse.Exception = ex;
                serviceResponse.Success = false;
                serviceResponse.Message = Resource.PostNotUpdated;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<String>> DeletePost(string slug)
        {
            var serviceResponse = new ServiceResponse<String>();
            try{
                Post post = await _context.Posts.Include(p => p.Tags).SingleAsync(post => post.Slug.Equals(slug));
                DeleteTags(post.Tags);
                _context.Posts.Remove(post);
                _context.SaveChanges();
                serviceResponse.Message = Resource.PostDeleted;
            }catch(Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = Resource.PostNotFound;
                serviceResponse.Exception = ex;
            }
            return serviceResponse;
        }

        private List<Post> GetPostsByTag(string tagName)
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
            }
            catch (Exception)
            {
                return posts;
            }
        }
        public List<int> AddTags(List<string>? tagNames)
        {
            List<int> tagIds = new();
            if (tagNames == null)
                return tagIds;

            foreach (string tagName in tagNames)
            {
                var t = _context.Tags.SingleOrDefault(t => t.Name.Equals(tagName));
                if (t == null)
                {
                    _context.Tags.Add(new Tag { Name = tagName });
                    _context.SaveChanges();
                    t = _context.Tags.SingleOrDefault(t => t.Name.Equals(tagName));
                }
                if (t != null)
                    tagIds.Add(t.Id);
            }

            return tagIds;
        }

        public void AddTagsToPost(int postId, List<int> tagIds)
        {
            var post = _context.Posts.Find(postId);
            if (post == null)
                throw new Exception("Post ne postoji!");

            foreach (int id in tagIds)
            {
                var tag = _context.Tags.FirstOrDefault(t => t.Id == id);
                if (tag != null)
                {
                    post.Tags.Add(tag);
                }
            }
            _context.SaveChanges();
        }

        public void DeleteTags(List<Tag> tags)
        {
            foreach (Tag tag in tags)
            {
                int postsNumber = _context.Tags.Include(t => t.Posts).Single(t => t.Id == tag.Id).Posts.Count;
                if (postsNumber <= 1)
                    _context.Remove(tag);
            }
            _context.SaveChanges();
        }
    }
}