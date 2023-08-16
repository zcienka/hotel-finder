using AutoMapper;
using Backend.Controllers;
using Backend.Interfaces;
using Backend.Models;
using Backend.Requests;
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
        var hotel = DataGenerator.GenerateHotel();

        string hotelId = hotel.Id;
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
    public async Task PutHotel_WhenHotelExists_ReturnsNoContent()
    {
        // Arrange
        var hotelId = "hotel123";
        var hotelRequest = DataGenerator.GenerateHotelRequest(hotelId);

        var mockHotelRepository = new Mock<IHotelRepository>();
        mockHotelRepository
            .Setup(repo => repo.Exists(hotelId))
            .Returns(true);

        var hotelController = new HotelsController(mockHotelRepository.Object, _mapper);

        // Act
        var result = await hotelController.PutHotel(hotelId, hotelRequest);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task PutHotel_WhenHotelDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var hotelId = "hotel123";

        var mockHotelRepository = new Mock<IHotelRepository>();
        mockHotelRepository.Setup(repo => repo.Update(It.IsAny<Hotel>()))
            .Throws(new DbUpdateConcurrencyException());

        var hotelController = new HotelsController(mockHotelRepository.Object, _mapper);

        // Act
        var result = await hotelController.PutHotel(hotelId, It.IsAny<HotelRequest>());

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }
}