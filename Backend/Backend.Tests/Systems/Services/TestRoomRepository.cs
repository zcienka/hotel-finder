namespace Backend.Tests.Systems.Services;

public class TestRoomRepository
{
    // [Fact]
    // public void GetAvailableRoomsById_ReturnsAvailableRooms()
    // {
    //     // Arrange
    //     var hotelId = "hotel123";
    //
    //     var reservedRoomIds = new List<string> { "room9", "room7" };
    //
    //     var reservations = new List<Reservation>
    //     {
    //         DataGenerator.GenerateReservation(hotelId, "room9", new DateTime(2023, 6, 4), new DateTime(2023, 6, 6)),
    //         DataGenerator.GenerateReservation(hotelId, "room7", new DateTime(2023, 6, 2), new DateTime(2023, 6, 3))
    //     };
    //
    //     var mockReservationRepository = new Mock<IReservationRepository>();
    //     mockReservationRepository
    //         .Setup(repo => repo.GetReservationsForHotel(hotelId))
    //         .ReturnsAsync(reservations);
    //
    //     var mockContext = new Mock<ApplicationDbContext>();
    //     var availableRooms = new List<Room>
    //     {
    //         DataGenerator.GenerateRoom(hotelId, "room1"),
    //         DataGenerator.GenerateRoom(hotelId, "room2"),
    //     };
    //
    //     var mockRoomSet = new Mock<DbSet<Room>>();
    //     mockContext.Setup(c => c.Rooms).Returns(mockRoomSet.Object);
    //
    //     var mockReservationSet = new Mock<DbSet<Reservation>>();
    //     mockContext.Setup(c => c.Reservations).Returns(mockReservationSet.Object);
    //
    //     var roomRepository = new RoomRepository(mockContext.Object);
    //
    //     // Act
    //     var result = await roomRepository.GetAvailableRoomsById(hotelId);
    //
    //     // Assert
    //     // Assert.Equal(2, result.Count);
    //     // mockReservationRepository.Verify();
    //     Assert.True(result.All(room => reservedRoomIds.Contains(room.Id) == false));
    //     // Assert.True(result.All(room => room.HotelId == hotelId));
    // }



}