using AutoMapper;
using Backend.Controllers;
using Backend.Interfaces;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
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
        public async Task PutComment_WhenValidData_ReturnsNoContent()
        {
            // Arrange
            var commentId = "comment-1";
            var hotelId = "hotel-1";
            var userEmail = "user@example.com";
            var commentDto = DataGenerator.GenerateCommentDto(userEmail, hotelId, commentId);

            var mockCommentRepository = new Mock<ICommentRepository>();

            mockCommentRepository
                .Setup(repo => repo.HotelExists(hotelId))
                .Returns(true);
            mockCommentRepository
            .Setup(repo => repo.UserExists(userEmail))
                .Returns(true);

            var commentController = new CommentsController(mockCommentRepository.Object, _mapper);

            // Act
            var result = await commentController.PutComment(commentId, commentDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PutComment_WhenHotelNotFound_ReturnsNotFound()
        {
            // Arrange
            var commentId = "comment-1";
            var hotelId = "hotel-1";
            var userEmail = "user@example.com";
            var commentDto = DataGenerator.GenerateCommentDto(userEmail, hotelId, commentId);

            var mockCommentRepository = new Mock<ICommentRepository>();

            mockCommentRepository
                .Setup(repo => repo.HotelExists(hotelId))
                .Returns(false);

            var commentController = new CommentsController(mockCommentRepository.Object, _mapper);

            // Act
            var result = await commentController.PutComment(commentId, commentDto);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            var notFoundResult = (NotFoundObjectResult)result;
            Assert.Equal("Hotel not found", notFoundResult.Value);
        }
    }
}
