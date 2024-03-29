using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace blog.Models.Responses
{
    public class MultiplePostServiceResponse<T>
    {
        public T? BlogPosts { get; set; }
        public int? PostsCount { get; set; }

        public bool Success { get; set; } = true;

        public string Message { get; set; } = string.Empty;

        public Exception? Exception = null;
    }
}