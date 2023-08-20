using AutoMapper;
using Backend.Controllers;
using Backend.Core.IConfiguration;
using Backend.Core.IRepositories;
using Backend.Data;
using Backend.Dtos;
using Backend.Models;
using Backend.Requests;
using Backend.Responses;
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
    public async Task GetHotels_ValidRequest_ReturnsHotels()
    {
        // Arrange
        var mock = new Mock<IUnitOfWork>();
        var availableHotels = new List<Hotel>
        {
            DataGenerator.GenerateHotel( "2"),
            DataGenerator.GenerateHotel("3"),
        }.AsEnumerable();

        mock.Setup(uow => uow.Hotels.GetSearchResults(null, null, null, null, null, null)).Returns(availableHotels);
        var HotelsController = new HotelsController(mock.Object, _mapper);

        // Act
        var result = await HotelsController.GetHotels(new PagingQuery { Limit = "10", Offset = "0" }, null, null, null, null, null, null);

        // Assert
        Assert.IsType<ActionResult<ApiResult<HotelResponse>>>(result);

        var apiResult = result.Value.Count;
        Assert.Equal(2, apiResult);
    }
    
    [Fact]
    public void GetHotel_HotelNotFound_ReturnsNotFound()
    {
        // Arrange
        string hotelId = "1";
        var mock = new Mock<IUnitOfWork>();
        mock.Setup(uow => uow.Hotels.Exists(hotelId)).ReturnsAsync(false);
        var hotelsController = new HotelsController(mock.Object, _mapper);
    
        // Act
        var result = hotelsController.GetHotel(hotelId);
    
        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result.Result);
    }
    
    [Fact]
    public void GetHotel_HotelFound_ReturnsHotelResponse()
    {
        // Arrange
        string hotelId = "1";
        var hotel = DataGenerator.GenerateHotel(hotelId);
    
        var mock = new Mock<IUnitOfWork>();
        mock.Setup(uow => uow.Hotels.GetById(hotelId)).ReturnsAsync(hotel);
        mock.Setup(uow => uow.Hotels.Exists(hotelId)).ReturnsAsync(true);
        var hotelsController = new HotelsController(mock.Object, _mapper);
    
        // Act
        var result = hotelsController.GetHotel(hotelId);
    
        // Assert
        Assert.IsType<ActionResult<HotelResponse>>(result.Result);
        var hotelDto = result.Result.Value;
        Assert.NotNull(hotelDto);
    }
    
    [Fact]
    public async Task PutHotel_ValidUpdate_ReturnsNoContent()
    {
        // Arrange
        string hotelId = "1";
        var hotelRequest = DataGenerator.GenerateHotelRequest(hotelId);
        var hotel = DataGenerator.GenerateHotel(hotelId);
    
        var mock = new Mock<IUnitOfWork>();
        mock.Setup(uow => uow.Hotels.Exists(hotelRequest.Id)).ReturnsAsync(true);
        mock.Setup(uow => uow.Hotels.Update(hotel)).Returns(true);
        mock.Setup(uow => uow.Users.Exists("1")).ReturnsAsync(true);
        var hotelsController = new HotelsController(mock.Object, _mapper);
    
        // Act
        var result = await hotelsController.PutHotel(hotelId, hotelRequest);
    
        // Assert
        Assert.IsType<NoContentResult>(result);
    }
    
    [Fact]
    public async Task PutHotel_InvalidId_ReturnsBadRequest()
    {
        // Arrange
        string hotelId = "1";
        string userEmail = "1";
        var mock = new Mock<IUnitOfWork>();
    
        var hotelRequest = DataGenerator.GenerateHotelRequest(hotelId);
        var hotelsController = new HotelsController(mock.Object, _mapper);
    
        mock.Setup(uow => uow.Users.Exists("1")).ReturnsAsync(true);
        mock.Setup(uow => uow.Hotels.Exists(hotelId)).ReturnsAsync(false);
    
        // Act
        var result = await hotelsController.PutHotel("2", hotelRequest);
    
        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
    
    [Fact]
    public async Task PostHotel_ValidHotel_ReturnsCreatedResponse()
    {
        // Arrange
        string hotelId = "1";
    
        var hotelRequest = DataGenerator.GenerateHotelRequest(hotelId);
        var mock = new Mock<IUnitOfWork>();
        var hotelsController = new HotelsController(mock.Object, _mapper);
    
        mock.Setup(uow => uow.Hotels.Exists(hotelId)).ReturnsAsync(true);
    
        // Act
        var result = await hotelsController.PostHotel(hotelRequest);
    
        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(nameof(hotelsController.GetHotel), createdAtActionResult.ActionName);
    }
    

    [Fact]
    public async Task DeleteHotel_HotelFound_ReturnsNoContent()
    {
        // Arrange
        string hotelId = "1";
    
        var mock = new Mock<IUnitOfWork>();
        var HotelsController = new HotelsController(mock.Object, _mapper);
    
        mock.Setup(uow => uow.Hotels.Exists(hotelId)).ReturnsAsync(true);
    
        // Act
        var result = await HotelsController.DeleteHotel(hotelId);
    
        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteHotel_HotelNotFound_ReturnsNotFound()
    {
        // Arrange
        string hotelId = "1";
    
        var mock = new Mock<IUnitOfWork>();
        var HotelsController = new HotelsController(mock.Object, _mapper);
    
        mock.Setup(uow => uow.Hotels.Exists(hotelId)).ReturnsAsync(false);
    
        // Act
        var result = await HotelsController.DeleteHotel(hotelId);
    
        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }
}