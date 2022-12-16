using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rubicon_blog.Models
{
    public class MultipleTagServiceResponse<T>
    {
        public T? Tags{get; set;}

        public bool Success{get;set;} = true;

        public string Message{get;set;} = string.Empty;

        public Exception? Exception = null;
    }
}