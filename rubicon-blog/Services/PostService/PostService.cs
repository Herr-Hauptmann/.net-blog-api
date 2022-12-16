using Slugify;
using rubicon_blog.Dtos.Post;
using Microsoft.EntityFrameworkCore;
using rubicon_blog.Services.TagService;
using rubicon_blog.Resources;

namespace rubicon_blog.Services.PostService
{
    public class PostService : IPostService
    {
        private readonly SlugHelper _slugHelper;
        private readonly DataContext _context;
        private readonly ITagService _tagService;

        public PostService(ITagService tagService, DataContext context)
        {
            _slugHelper = new SlugHelper();
            _context = context;
            _tagService = tagService;
        }

        public async Task<SinglePostServiceResponse<GetPostDto>> AddPost(AddPostDto newPost)
        {
            var serviceResponse = new SinglePostServiceResponse<GetPostDto>();
            try
            {
                //Create post
                Post post = Helpers.Mapper.MapAddDtoToPost(newPost);
                post.Slug = _slugHelper.GenerateSlug(newPost.Title);
                var addedPost = await _context.Posts.AddAsync(post);
                _context.SaveChanges();

                //Create tags and add them to post
                List<int> tagIds = _tagService.AddTags(newPost.TagList);
                _tagService.AddTagsToPost(addedPost.Entity.Id, tagIds);

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
                    posts = _tagService.GetPostsByTag(tagName);
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
                _tagService.DeleteTags(post.Tags);
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
    }
}