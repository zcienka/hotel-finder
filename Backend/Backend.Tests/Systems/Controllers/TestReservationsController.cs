using AutoMapper;
using Backend.Controllers;
using Backend.Core.IConfiguration;
using Backend.Data;
using Backend.Dtos;
using Backend.Models;
using Backend.Tests.Systems.Mocks;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Backend.Tests.Systems.Controllers;

public class TestReservationsController
{
    private readonly IMapper _mapper;

    public TestReservationsController()
    {
        _mapper = A.Fake<IMapper>();
    }

    [Fact]
    public async Task GetReservations_ValidRequest_ReturnsReservations()
    {
        // Arrange
        var mock = new Mock<IUnitOfWork>();
        string hotelId = "1";
        string roomId = "2";
        DateTime checkInDate = new DateTime(2023, 6, 4);
        DateTime checkOutDate = new DateTime(2023, 6, 5);

        var availableReservations = new List<Reservation>
        {
            DataGenerator.GenerateReservation(hotelId, roomId, checkInDate, checkOutDate),
            DataGenerator.GenerateReservation(hotelId, roomId, checkInDate, checkOutDate),
        }.AsEnumerable();

        mock.Setup(uow => uow.Reservations.GetAll()).ReturnsAsync(availableReservations);
        var reservationsController = new ReservationsController(mock.Object, _mapper);

        // Act
        var result = await reservationsController.GetReservations(new PagingQuery { Limit = "10", Offset = "0" });

        // Assert
        Assert.IsType<ActionResult<ApiResult<ReservationDto>>>(result);

        var apiResult = result.Value.Count;
        Assert.Equal(2, apiResult);
    }
 
}