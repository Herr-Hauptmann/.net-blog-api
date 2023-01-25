using Moq;
using FluentAssertions;
using blog.Controllers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using blog.Services.TagServices;
using blog.Models.Responses;

namespace BlogTests
{
    public class TagsTest
    {
        
        [Fact]
        public async Task GetAllTagsReturnsStatus200()
        {
            var mockTagService = new Moq.Mock<ITagService>();
            mockTagService.Setup(service => service.GetAllTags()).ReturnsAsync(new MultipleTagServiceResponse<List<string>>());

            var sut = new TagsController(mockTagService.Object);

            var result = (await sut.Get()).Result as OkObjectResult;

            result.StatusCode.Should().Be(200);
        }
        [Fact]
        public async Task GetAllTagsReturnsListOfTags()
        {
            var tags = new MultipleTagServiceResponse<List<string>>
            {
                Tags = new List<string> { "Tag 1", "Tag 2", "Tag 3" }
            };
            var mockTagService = new Moq.Mock<ITagService>();
            mockTagService.Setup(service => service.GetAllTags()).ReturnsAsync(tags);

            var sut = new TagsController(mockTagService.Object);

            var result = (await sut.Get()).Result as OkObjectResult;

            result.Value.Should().Be(tags);
        }
    }
}