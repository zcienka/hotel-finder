using AutoMapper;
using Backend.Controllers;
using Backend.Data;
using Backend.Dtos;
using Backend.Interfaces;
using Backend.Models;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Backend.Tests.Systems.Controllers;

public class TestHotelController
{
    private readonly IMapper _mapper;

    public TestHotelController()
    {
        _mapper = A.Fake<IMapper>();
    }

    [Fact]
    public async Task DeleteHotel_WhenCommentsExist_ReturnsNoContent()
    {
        // Arrange
        var mockCommentSet = new Mock<DbSet<Comment>>();
        var hotel = DataGenerator.GenerateHotel();

        string hotelId = hotel.Id;
        int userId = 2;
        int commentId = 3;

        var mockHotelRepository = new Mock<IHotelRepository>();

        mockHotelRepository
            .Setup(repo => repo.GetByIdAsync(hotelId))
            .ReturnsAsync(hotel);

        var hotelController = new HotelsController(mockHotelRepository.Object, _mapper);

        // Act
        var result = await hotelController.DeleteHotel(hotelId);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }
    
    
    [Fact]
    public async Task GetCommentsByHotel_ExistingHotelId_ReturnsOkResultWithComments()
    {
        // Arrange
        var hotel = DataGenerator.GenerateHotel();
        var userEmail = "example@example.com";
        var hotels = new List<Hotel> { hotel }.AsQueryable();
        string hotelId = hotel.Id;
    
        var comments = new List<Comment>
        {
            DataGenerator.GenerateComment(userEmail, hotelId),
            DataGenerator.GenerateComment(userEmail, hotelId),
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
}