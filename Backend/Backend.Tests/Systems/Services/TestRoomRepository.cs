using Backend.Core.IRepositories;
using Backend.Core.Repositories;
using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Backend.Tests.Systems.Services;

public class TestRoomRepository
{
    [Fact]
    public void GetAvailableRoomsById_ReturnsAvailableRooms()
    {
        // Arrange
        var hotelId = "hotel123";
    
        var reservedRoomIds = new List<string> { "room9", "room7" };
    
        var reservations = new List<Reservation>
        {
            DataGenerator.GenerateReservation(hotelId, "room9", new DateTime(2023, 6, 4), new DateTime(2023, 6, 6)),
            DataGenerator.GenerateReservation(hotelId, "room7", new DateTime(2023, 6, 2), new DateTime(2023, 6, 3))
        };
    
        var mockContext = new Mock<ApplicationDbContext>();
        var availableRooms = new List<Room>
        {
            DataGenerator.GenerateRoom(hotelId, "room1"),
            DataGenerator.GenerateRoom(hotelId, "room2"),
        };
    
        var mockRoomSet = new Mock<DbSet<Room>>();
        mockRoomSet.SetupIQueryable(availableRooms.AsQueryable());
        mockContext.Setup(c => c.Rooms).Returns(mockRoomSet.Object);
    
        var mockReservationSet = new Mock<DbSet<Reservation>>();
        mockReservationSet.SetupIQueryable(reservations.AsQueryable());
        mockContext.Setup(c => c.Reservations).Returns(mockReservationSet.Object);
    
        var roomRepository = new RoomRepository(mockContext.Object);
    
        // Act
        var result = roomRepository.GetAvailableRoomsById(hotelId);
    
        // Assert
        Assert.Equal(2, result.Count());
        Assert.True(result.All(room => reservedRoomIds.Contains(room.Id) == false));
        Assert.True(result.All(room => room.HotelId == hotelId));
    }

}