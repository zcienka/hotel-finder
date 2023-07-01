using AutoMapper;
using Backend.Controllers;
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
    public async Task DeleteHotel_WhenCommentsExist_RemovesComments()
    {
        // Arrange


        var mockContext = new Mock<ApplicationDbContext>();

        var mockHotelSet = new Mock<DbSet<Hotel>>();
        var mockCommentSet = new Mock<DbSet<Comment>>();
        var mockReservationSet = new Mock<DbSet<Reservation>>();
        var mockRoomSet = new Mock<DbSet<Room>>();
        var hotel = DataGenerator.GenerateHotel();

        string hotelId = hotel.Id;
        int userId = 2;
        int commentId = 3;

        var hotels = new List<Hotel> { hotel }.AsQueryable();
        var comments = new List<Comment>
        {
            DataGenerator.GenerateComment("userId", hotelId),
            DataGenerator.GenerateComment("userId", hotelId)
        }.AsQueryable();

        var reservations = new List<Reservation>().AsQueryable();
        var rooms = new List<Room>().AsQueryable();

        mockHotelSet.SetupIQueryable(hotels);
        mockCommentSet.SetupIQueryable(comments);
        mockReservationSet.SetupIQueryable(reservations);
        mockRoomSet.SetupIQueryable(rooms);

        mockContext.Setup(c => c.Hotels).Returns(mockHotelSet.Object);
        mockContext.Setup(c => c.Comments).Returns(mockCommentSet.Object);
        mockContext.Setup(c => c.Reservations).Returns(mockReservationSet.Object);
        mockContext.Setup(c => c.Rooms).Returns(mockRoomSet.Object);

        var hotelController = new HotelsController(mockContext.Object, _mapper);

        // Act
        var result = await hotelController.DeleteHotel(hotelId);

        // Assert
        Assert.IsType<NoContentResult>(result);
        mockCommentSet.Verify(x => x.RemoveRange(It.IsAny<IEnumerable<Comment>>()));
    }

    [Fact]
    public async Task DeleteHotel_WhenCommentsExist_RemovesReservations()
    {
        // Arrange
        int userId = 2;
        int commentId = 3;

        var mockContext = new Mock<ApplicationDbContext>();

        var mockHotelSet = new Mock<DbSet<Hotel>>();
        var mockCommentSet = new Mock<DbSet<Comment>>();
        var mockReservationSet = new Mock<DbSet<Reservation>>();
        var mockRoomSet = new Mock<DbSet<Room>>();
        var hotel = DataGenerator.GenerateHotel();
        string hotelId = hotel.Id;

        var hotels = new List<Hotel> { hotel }.AsQueryable();
        var comments = new List<Comment>
        {
            DataGenerator.GenerateComment("userId", hotelId),
            DataGenerator.GenerateComment("userId", hotelId)
        }.AsQueryable();

        var room = DataGenerator.GenerateRoom(hotelId);
        var rooms = new List<Room> { room }.AsQueryable();
        string roomId = room.Id;

        var reservations = new List<Reservation>()
        {
            DataGenerator.GenerateReservation(hotelId, roomId),
            DataGenerator.GenerateReservation(hotelId, roomId)
        }.AsQueryable();

        mockHotelSet.SetupIQueryable(hotels);
        mockCommentSet.SetupIQueryable(comments);
        mockReservationSet.SetupIQueryable(reservations);
        mockRoomSet.SetupIQueryable(rooms);

        mockContext.Setup(c => c.Hotels).Returns(mockHotelSet.Object);
        mockContext.Setup(c => c.Comments).Returns(mockCommentSet.Object);
        mockContext.Setup(c => c.Reservations).Returns(mockReservationSet.Object);
        mockContext.Setup(c => c.Rooms).Returns(mockRoomSet.Object);

        var hotelController = new HotelsController(mockContext.Object, _mapper);

        // Act
        var result = await hotelController.DeleteHotel(hotelId);

        // Assert
        Assert.IsType<NoContentResult>(result);
        mockReservationSet.Verify(x => x.RemoveRange(It.IsAny<IEnumerable<Reservation>>()));
    }

    [Fact]
    public async Task DeleteHotel_WhenCommentsExist_RemovesRooms()
    {
        // Arrange
        int userId = 2;
        int commentId = 3;
    
        var mockContext = new Mock<ApplicationDbContext>();
    
        var mockHotelSet = new Mock<DbSet<Hotel>>();
        var mockCommentSet = new Mock<DbSet<Comment>>();
        var mockReservationSet = new Mock<DbSet<Reservation>>();
        var mockRoomSet = new Mock<DbSet<Room>>();

        var hotel1 = DataGenerator.GenerateHotel();
        var hotel2 = DataGenerator.GenerateHotel();

        var hotels = new List<Hotel> { hotel1, hotel2 }.AsQueryable();
        string hotel1Id = hotel1.Id;
        string hotel2Id = hotel2.Id;

        var comments = new List<Comment>().AsQueryable();
        var reservations = new List<Reservation>().AsQueryable();
        var rooms = new List<Room>()
        {
            DataGenerator.GenerateRoom(hotel1Id),
            DataGenerator.GenerateRoom(hotel2Id)
        }.AsQueryable();
    
        mockHotelSet.SetupIQueryable(hotels);
        mockCommentSet.SetupIQueryable(comments);
        mockReservationSet.SetupIQueryable(reservations);
        mockRoomSet.SetupIQueryable(rooms);
    
        mockContext.Setup(c => c.Hotels).Returns(mockHotelSet.Object);
        mockContext.Setup(c => c.Comments).Returns(mockCommentSet.Object);
        mockContext.Setup(c => c.Reservations).Returns(mockReservationSet.Object);
        mockContext.Setup(c => c.Rooms).Returns(mockRoomSet.Object);
    
        var hotelController = new HotelsController(mockContext.Object, _mapper);
    
        // Act
        var result = await hotelController.DeleteHotel(hotel1Id);
    
        // Assert
        Assert.IsType<NoContentResult>(result);
        mockRoomSet.Verify(x => x.RemoveRange(It.IsAny<IEnumerable<Room>>()));
    }
}