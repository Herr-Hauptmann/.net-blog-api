using blog.Dtos.Comment;
using blog.Dtos.Post;
using blog.Services.CommentService;
using blog.Services.PostService;

namespace blog.Data
{
    public class DataSeeder
    {
        private readonly DataContext _context;
        private readonly IPostService _postService;
        private readonly ICommentService _commentService;
        public DataSeeder(DataContext context, IPostService postService, ICommentService commentService)
        {
            _context = context;
            _postService = postService;
            _commentService = commentService;
        }
        public void Seed()
        {
            if (_context.Posts.Any()) return;
            var postsDtos = new List<AddPostDto>
            {
                new AddPostDto
                {
                    Title = "A plasma physicist explains what’s next after this week’s nuclear fusion breakthrough",
                    Description = "Scientists need better targets to shoot and even more advanced lasers.",
                    Body = "Tammy Ma was about to board a plane at the San Francisco International Airport when she got the call of a lifetime.",
                    TagList = new List<string>
                    {
                        "Science",
                        "Energy",
                        "Environment"
                    }
                },
                new AddPostDto
                {
                    Title = "What in the world is nuclear fusion — and when will we harness it?",
                    Description = "Fusion power could revolutionize our energy system. But after decades of research, it’s still out of reach.",
                    Body = "Nuclear fusion is back in the news.",
                    TagList = new List<string>
                    {
                        "Science",
                        "Energy",
                        "Climate"
                    }
                },
                new AddPostDto
                {
                    Title = "HTC will announce a lightweight Meta Quest competitor at CES",
                    Description = "A first look at the unnamed device, which will feature color passthrough mixed reality.",
                    Body = "HTC plans to introduce a new flagship AR / VR headset next month that will reestablish its presence in the consumer virtual reality space.",
                    TagList = new List<string>
                    {
                        "HTC",
                        "Tech",
                        "Virtual reality"
                    }
                }
            };

            foreach(var p in postsDtos)
            {
                _postService.AddPost(p);
            }

            var createdPosts = _context.Posts.ToList();
            int i = 1;
            foreach(var post in createdPosts)
            {
                for (int j = 1; j <= i; j++)
                    _commentService.AddComment(
                        post.Slug,
                        new AddCommentDto
                        {
                            Body = "Neki random komentar br. " + j
                        }
                    );
                i++;
            }
        }
    }
}
