using AutoMapper;
using Backend.Controllers;
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
}