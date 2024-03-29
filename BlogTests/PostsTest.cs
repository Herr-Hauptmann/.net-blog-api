﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using blog.Controllers;
using blog.Dtos.Post;
using blog.Services.CommentService;
using blog.Services.PostService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using blog.Models.Responses;

namespace BlogTests
{
    public class PostTests
    {

        [Fact]
        public async Task GetAllTPostsReturnsStatus200()
        {
            var mockPostService = new Mock<IPostService>();
            var mockCommentService = new Mock<ICommentService>();
            mockPostService.Setup(service => service.GetAllPosts("")).ReturnsAsync(new MultiplePostServiceResponse<List<GetPostDto>>
            {
                BlogPosts = new List<GetPostDto>()
            });

            var sut = new PostsController(mockPostService.Object, mockCommentService.Object);

            OkObjectResult? result = (await sut.Get()).Result as OkObjectResult;

            result.StatusCode.Should().Be(200);
        }
        [Fact]
        public async Task GetAllTagsReturnsMultiplePostServiceResponse()
        {
            var posts = new MultiplePostServiceResponse<List<GetPostDto>>
            {
                BlogPosts = new List<GetPostDto>
                {
                    new GetPostDto{
                        Title = "Naslov",
                        Description = "Opis",
                        Body = "Tijelo",
                        tagList = { "Tag 1", "Tag 2" },
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        Slug = "naslov"
                    }
                },
                Message = "",
                Success = true,
                Exception = null,
                PostsCount = 1,
            };
            var mockPostService = new Moq.Mock<IPostService>();
            var mockCommentService = new Moq.Mock<ICommentService>();
            mockPostService.Setup(service => service.GetAllPosts("")).ReturnsAsync(posts);

            var sut = new PostsController(mockPostService.Object, mockCommentService.Object);

            var res = await sut.Get();
            OkObjectResult? result = res.Result as OkObjectResult;

            result.Value.Should().Be(posts);
        }
    }
}
