using AutoMapper;
using Backend.Controllers;
using Backend.Models;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
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
                DataGenerator.GenerateHotel(hotelId)
            }.AsQueryable();

            var comments = new List<Comment>
            {
                new Comment
                {
                    Id = 1,
                    Description = "",
                    UserId = 1,
                    HotelId = hotelId
                },
                new Comment
                {
                    Id = 2,
                    Description = "",
                    UserId = 1,
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

            var controller = new CommentsController(mockContext.Object, _mapper);

            // Act
            var result = await controller.GetCommentsByHotel(hotelId, new PagingQuery { Limit = "10", Offset = "0" });

            // Assert
            Assert.IsType<ActionResult<ApiResult<CommentDto>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResult = Assert.IsType<ApiResult<CommentDto>>(okResult.Value);
            Assert.Equal(2, apiResult.Results.Count);
        }


        [Fact]
        public async Task PostComment_NonExistentHotelId_ReturnsNotFound()
        {
            // Arrange
            var hotelId = 100;
            var comment = new CommentDto
            {
                Description = "",
                UserEmail = "",
                HotelId = hotelId
            };
            var comments = new List<Comment>().AsQueryable();
            var hotels = new List<Hotel>().AsQueryable();

            var mockContext = new Mock<ApplicationDbContext>();
            var mockSetHotel = new Mock<DbSet<Hotel>>();
            var mockSetComment = new Mock<DbSet<Comment>>();

            mockSetHotel.SetupIQueryable(hotels);
            mockSetComment.SetupIQueryable(comments);

            mockContext.SetupGet(m => m.Hotels).Returns(mockSetHotel.Object);
            mockContext.SetupGet(m => m.Comments).Returns(mockSetComment.Object);

            var commentsController = new CommentsController(mockContext.Object, _mapper);

            // Act
            var result = await commentsController.PostComment(comment);

            // Assert        
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Hotel not found", notFoundResult.Value);
        }

        [Fact]
        public async Task PostComment_ExistingHotel_ReturnsOk()
        {
            // Arrange
            var hotelId = 100;
            var hotels = new List<Hotel>
            {
                DataGenerator.GenerateHotel(hotelId)
            }.AsQueryable();

            var comment = new CommentDto
            {
                Description = "This is a comment",
                UserEmail = "test@example.com",
                HotelId = hotelId
            };
            var comments = new List<Comment>().AsQueryable();

            var mockContext = new Mock<ApplicationDbContext>();

            var mockHotelSet = new Mock<DbSet<Hotel>>();
            var mockCommentSet = new Mock<DbSet<Comment>>();

            mockHotelSet.SetupIQueryable(hotels);
            mockCommentSet.SetupIQueryable(comments);

            mockContext.SetupGet(m => m.Hotels).Returns(mockHotelSet.Object);
            mockContext.SetupGet(m => m.Comments).Returns(mockCommentSet.Object);

            var commentsController = new CommentsController(mockContext.Object, _mapper);

            // Act
            var result = await commentsController.PostComment(comment);

            // Assert        
            var okResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(comment, okResult.Value);
        }
    }
}
