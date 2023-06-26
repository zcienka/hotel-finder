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
        int hotelId = 1;
        int userId = 2;
        int commentId = 3;

        var mockContext = new Mock<ApplicationDbContext>();

        var mockHotelSet = new Mock<DbSet<Hotel>>();
        var mockCommentSet = new Mock<DbSet<Comment>>();
        var mockReservationSet = new Mock<DbSet<Reservation>>();
        var mockRoomSet = new Mock<DbSet<Room>>();

        var hotels = new List<Hotel> { DataGenerator.GenerateHotel(hotelId) }.AsQueryable();
        var comments = new List<Comment>
        {
            DataGenerator.GenerateComment(commentId, userId, hotelId),
            DataGenerator.GenerateComment(commentId, userId, hotelId)
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
        int hotelId = 1;
        int userId = 2;
        int commentId = 3;
        int roomId = 4;

        var mockContext = new Mock<ApplicationDbContext>();

        var mockHotelSet = new Mock<DbSet<Hotel>>();
        var mockCommentSet = new Mock<DbSet<Comment>>();
        var mockReservationSet = new Mock<DbSet<Reservation>>();
        var mockRoomSet = new Mock<DbSet<Room>>();

        var hotels = new List<Hotel> { DataGenerator.GenerateHotel(hotelId) }.AsQueryable();
        var comments = new List<Comment>
        {
            DataGenerator.GenerateComment(commentId, userId, hotelId),
            DataGenerator.GenerateComment(commentId, userId, hotelId)
        }.AsQueryable();

        var reservations = new List<Reservation>()
        {
            DataGenerator.GenerateReservation(1, hotelId, roomId),
            DataGenerator.GenerateReservation(2, hotelId, roomId)
        }.AsQueryable();
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
        mockReservationSet.Verify(x => x.RemoveRange(It.IsAny<IEnumerable<Reservation>>()));
    }

    [Fact]
    public async Task DeleteHotel_WhenCommentsExist_RemovesRooms()
    {
        // Arrange
        int hotelId = 1;
        int userId = 2;
        int commentId = 3;
    
        var mockContext = new Mock<ApplicationDbContext>();
    
        var mockHotelSet = new Mock<DbSet<Hotel>>();
        var mockCommentSet = new Mock<DbSet<Comment>>();
        var mockReservationSet = new Mock<DbSet<Reservation>>();
        var mockRoomSet = new Mock<DbSet<Room>>();
    
        var hotels = new List<Hotel> { DataGenerator.GenerateHotel(hotelId) }.AsQueryable();
        var comments = new List<Comment>().AsQueryable();
        var reservations = new List<Reservation>().AsQueryable();
        var rooms = new List<Room>()
        {
            DataGenerator.GenerateRoom(1, hotelId),
            DataGenerator.GenerateRoom(2, hotelId)
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
        mockRoomSet.Verify(x => x.RemoveRange(It.IsAny<IEnumerable<Room>>()));
    }
}