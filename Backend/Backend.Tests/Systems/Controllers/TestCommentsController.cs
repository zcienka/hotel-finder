using AutoMapper;
using Backend.Controllers;
using Backend.Models;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Backend.Tests.Systems.Controllers
{
    public class TestCommentsController
    {
        private readonly IMapper _mapper;

        public TestCommentsController()
        {
            _mapper = A.Fake<IMapper>();
        }

        [Fact]
        public async Task GetCommentsByHotel_ExistingHotelId_ReturnsOkResultWithComments()
        {
            // Arrange
            int hotelId = 1;

            var hotels = new List<Hotel>
            {
                new Hotel
                {
                    Id = hotelId, Name = "Hotel 1", Description = "Description 1", Address = "Address 1",
                    City = "City 1", PhoneNumber = "123456789", Stars = 3
                }
            }.AsQueryable();

            var comments = new List<Comment>
            {
                new Comment
                {
                    Id = 1,
                    Description = "",
                    UserEmail = "",
                    HotelId = hotelId
                },
                new Comment
                {
                    Id = 2,
                    Description = "",
                    UserEmail = "",
                    HotelId = hotelId
                }
            }.AsQueryable();

            var mockContext = new Mock<ApplicationDbContext>();

            Mock<DbSet<Hotel>> mockSetHotel = new Mock<DbSet<Hotel>>();
            mockSetHotel.SetupIQueryable(hotels);
            mockContext.SetupGet(m => m.Hotels).Returns(mockSetHotel.Object);


            Mock<DbSet<Comment>> mockSetComment = new Mock<DbSet<Comment>>();
            mockSetComment.SetupIQueryable(comments);
            mockContext.SetupGet(m => m.Comments).Returns(mockSetComment.Object);

            var _controller = new CommentsController(mockContext.Object, _mapper);

            // Act
            var result = await _controller.GetCommentsByHotel(hotelId, new PagingQuery { Limit = "10", Offset = "0" });

            // Assert
            Assert.IsType<ActionResult<ApiResult<CommentDto>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResult = Assert.IsType<ApiResult<CommentDto>>(okResult.Value);
            Assert.Equal(2, apiResult.Results.Count);
        }
    }
}