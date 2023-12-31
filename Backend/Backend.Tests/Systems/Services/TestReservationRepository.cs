﻿using Backend.Core.Repositories;
using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Backend.Tests.Systems.Services;

public class TestReservationRepository
{
    [Fact]
    public void GetReservationsForHotel_ShouldReturnValidReservations()
    {
        // Arrange
        var hotelId = "1";

        var mockContext = new Mock<ApplicationDbContext>();
        var mockReservationSet = new Mock<DbSet<Reservation>>();

        int userId = 2;
        int commentId = 3;
        string roomId = "3";

        var reservations = new List<Reservation>
        {
            new()
            {
                Id = "1", HotelId = hotelId, CheckInDate = DateTime.Now.AddDays(-1),
                CheckOutDate = DateTime.Now.AddDays(1), RoomId = roomId, UserEmail = "1"
            },
            new()
            {
                Id = "2", HotelId = hotelId, CheckInDate = DateTime.Now.AddDays(-2),
                CheckOutDate = DateTime.Now.AddDays(-1), RoomId = roomId, UserEmail = "1"
            },
            new()
            {
                Id = "3", HotelId = "2", CheckInDate = DateTime.Now.AddDays(-1), CheckOutDate = DateTime.Now.AddDays(1),
                RoomId = roomId, UserEmail = "1"
            },
        };

        mockReservationSet.SetupIQueryable(reservations.AsQueryable());
        mockContext.Setup(c => c.Reservations).Returns(mockReservationSet.Object);
        var reservationRepository = new ReservationRepository(mockContext.Object);

        // Act
        var result = reservationRepository.GetReservationsForHotel(hotelId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Count());
    }
}