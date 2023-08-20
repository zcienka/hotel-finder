using AutoMapper;
using Backend.Controllers;
using Backend.Core.IConfiguration;
using Backend.Core.IRepositories;
using Backend.Data;
using Backend.Dtos;
using Backend.Models;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Backend.Tests.Systems.Controllers;

public class TestCommentsController
{
    private readonly IMapper _mapper;

    public TestCommentsController()
    {
        _mapper = A.Fake<IMapper>();
    }

    [Fact]
    public async Task GetComments_ValidRequest_ReturnsComments()
    {
        // Arrange
        var mock = new Mock<IUnitOfWork>();
        string hotelId = "1";
        var availableComments = new List<Comment>
        {
            DataGenerator.GenerateComment("1", hotelId, "2"),
            DataGenerator.GenerateComment("1", hotelId, "3"),
        }.AsEnumerable();

        mock.Setup(uow => uow.Comments.GetAll()).ReturnsAsync(availableComments);
        var CommentsController = new CommentsController(mock.Object, _mapper);

        // Act
        var result = await CommentsController.GetComments(new PagingQuery { Limit = "10", Offset = "0" });

        // Assert
        Assert.IsType<ActionResult<ApiResult<CommentDto>>>(result);

        var apiResult = result.Value.Count;
        Assert.Equal(2, apiResult);
    }

    [Fact]
    public void GetComment_CommentNotFound_ReturnsNotFound()
    {
        // Arrange
        string commentId = "1";
        var mock = new Mock<IUnitOfWork>();
        mock.Setup(uow => uow.Comments.Exists(commentId)).ReturnsAsync(false);
        var commentsController = new CommentsController(mock.Object, _mapper);

        // Act
        var result = commentsController.GetComment(commentId);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result.Result);
    }

    [Fact]
    public void GetComment_CommentFound_ReturnsCommentDto()
    {
        // Arrange
        string commentId = "1";
        string hotelId = "1";
        var comment = DataGenerator.GenerateComment("1", hotelId, commentId);

        var mock = new Mock<IUnitOfWork>();
        mock.Setup(uow => uow.Comments.GetById(commentId)).ReturnsAsync(comment);
        mock.Setup(uow => uow.Comments.Exists(commentId)).ReturnsAsync(true);
        var commentsController = new CommentsController(mock.Object, _mapper);

        // Act
        var result = commentsController.GetComment(commentId);

        // Assert
        Assert.IsType<ActionResult<CommentDto>>(result.Result);
        var commentDto = result.Result.Value;
        Assert.NotNull(commentDto);
    }

    [Fact]
    public async Task PutComment_ValidUpdate_ReturnsNoContent()
    {
        // Arrange
        string commentId = "1";
        string hotelId = "1";
        var commentRequest = DataGenerator.GenerateCommentDto("1", hotelId, commentId);
        var comment = DataGenerator.GenerateComment("1", hotelId, commentId);
    
        var mock = new Mock<IUnitOfWork>();
        mock.Setup(uow => uow.Hotels.Exists(commentRequest.HotelId)).ReturnsAsync(true);
        mock.Setup(uow => uow.Comments.Update(comment)).Returns(true);
        mock.Setup(uow => uow.Users.Exists("1")).ReturnsAsync(true);
        mock.Setup(uow => uow.Comments.Exists(commentId)).ReturnsAsync(true);
        var commentsController = new CommentsController(mock.Object, _mapper);
    
        // Act
        var result = await commentsController.PutComment(commentId, commentRequest);
    
        // Assert
        Assert.IsType<NoContentResult>(result); 
    }
    
    [Fact]
    public async Task PutComment_InvalidId_ReturnsBadRequest()
    {
        // Arrange
        string commentId = "1";
        string hotelId = "1";
        string userEmail = "1";
        var mock = new Mock<IUnitOfWork>();

        var commentRequest = DataGenerator.GenerateCommentDto(userEmail, hotelId, commentId);
        var commentsController = new CommentsController(mock.Object, _mapper);

        mock.Setup(uow => uow.Hotels.Exists(commentRequest.HotelId)).ReturnsAsync(true);
        mock.Setup(uow => uow.Users.Exists("1")).ReturnsAsync(true);
        mock.Setup(uow => uow.Comments.Exists(commentId)).ReturnsAsync(false);

        // Act
        var result = await commentsController.PutComment("2", commentRequest);
    
        // Assert
        Assert.IsType<BadRequestResult>(result);
    }
    
    [Fact]
    public async Task PutComment_HotelNotFound_ReturnsNotFound()
    {
        // Arrange
        string commentId = "1";
        string hotelId = "1";
        string userEmail = "1";

        var commentRequest = DataGenerator.GenerateCommentDto(userEmail, hotelId, commentId);
        var mock = new Mock<IUnitOfWork>();
    
        var commentsController = new CommentsController(mock.Object, _mapper);
    
        mock.Setup(uow => uow.Hotels.Exists(commentRequest.HotelId)).ReturnsAsync(false);
    
        // Act
        var result = await commentsController.PutComment(commentId, commentRequest);
    
        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Hotel not found", notFoundResult.Value);
    }
    
    
    [Fact]
    public async Task PostComment_ValidComment_ReturnsCreatedResponse()
    {
        // Arrange
        string commentId = "1";
        string hotelId = "1";
        string userEmail = "1";

        var commentRequest = DataGenerator.GenerateCommentDto(userEmail, hotelId, commentId);
        var commment = DataGenerator.GenerateComment(userEmail, hotelId, commentId);
        var mock = new Mock<IUnitOfWork>();
        var commentsController = new CommentsController(mock.Object, _mapper);
    
        mock.Setup(uow => uow.Hotels.Exists(commentRequest.HotelId)).ReturnsAsync(true);
        mock.Setup(uow => uow.Users.Exists(userEmail)).ReturnsAsync(true);
        mock.Setup(uow => uow.Comments.Add(commment)).ReturnsAsync(true);
    
        // Act
        var result = await commentsController.PostComment(commentRequest);
    
        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(nameof(commentsController.GetComment), createdAtActionResult.ActionName);
    }
    
    
    [Fact]
    public async Task PostComment_HotelNotFound_ReturnsNotFound()
    {
        // Arrange
        string commentId = "1";
        string hotelId = "1";
        string userEmail = "1";

        var commentRequest = DataGenerator.GenerateCommentDto(userEmail, hotelId, commentId);
        var mock = new Mock<IUnitOfWork>();
        var commentsController = new CommentsController(mock.Object, _mapper);
    
        mock.Setup(uow => uow.Hotels.Exists(commentRequest.HotelId)).ReturnsAsync(false);
        mock.Setup(uow => uow.Users.Exists(userEmail)).ReturnsAsync(true);

        // Act
        var result = await commentsController.PostComment(commentRequest);
    
        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal("Hotel not found", notFoundResult.Value);
    }
    
    [Fact]
    public async Task DeleteComment_CommentFound_ReturnsNoContent()
    {
        // Arrange
        string commentId = "1";
        string hotelId = "1";
        string userEmail = "1";

        var CommentEntity = DataGenerator.GenerateComment(userEmail, hotelId, commentId);
        var mock = new Mock<IUnitOfWork>();
        var CommentsController = new CommentsController(mock.Object, _mapper);
    
        mock.Setup(uow => uow.Comments.GetById(commentId)).ReturnsAsync(CommentEntity);
    
        // Act
        var result = await CommentsController.DeleteComment(commentId);
    
        // Assert
        Assert.IsType<NoContentResult>(result);
    }
    
    [Fact]
    public async Task DeleteComment_CommentNotFound_ReturnsNotFound()
    {
        // Arrange
        string commentId = "1";
        string hotelId = "1";
    
        var mock = new Mock<IUnitOfWork>();
        var CommentsController = new CommentsController(mock.Object, _mapper);
    
        mock.Setup(uow => uow.Comments.Exists(commentId)).ReturnsAsync(false);
    
        // Act
        var result = await CommentsController.DeleteComment(commentId);
    
        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}