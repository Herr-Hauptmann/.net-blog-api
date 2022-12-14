using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using rubicon_blog.Dtos.Comment;
using rubicon_blog.Dtos.Post;

namespace rubicon_blog
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Post, GetPostDto>();
            CreateMap<AddPostDto, Post>();
            CreateMap<UpdatePostDto, Post>();
            
            CreateMap<AddCommentDto, Comment>();
            CreateMap<Comment, GetCommentDto>();
        }
    }
}