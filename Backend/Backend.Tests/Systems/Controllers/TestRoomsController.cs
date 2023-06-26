using AutoMapper;
using Backend.Controllers;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Backend.Tests.Systems;
using FakeItEasy;

namespace Backend.Tests.Controllers
{
    public class TestRoomsController
    {
        private readonly IMapper _mapper;

        public TestRoomsController()
        {
            _mapper = A.Fake<IMapper>();
        }

        [Fact]
        public async Task GetRoomsInHotel_WhenHotelExistsButNoRooms_ReturnsNotFound()
        {
            // Arrange
            int hotelId = 1;
            var hotels = new List<Hotel>
            {
                DataGenerator.GenerateHotel(hotelId)
            }.AsQueryable();
            var rooms = new List<Room>().AsQueryable();

            var mockHotelSet = new Mock<DbSet<Hotel>>();
            mockHotelSet.SetupIQueryable(hotels);

            var mockRoomSet = new Mock<DbSet<Room>>();
            mockRoomSet.SetupIQueryable(rooms);

            var mockContext = new Mock<ApplicationDbContext>();
            mockContext.SetupGet(m => m.Hotels).Returns(mockHotelSet.Object);
            mockContext.SetupGet(m => m.Rooms).Returns(mockRoomSet.Object);

            var roomsController = new RoomsController(mockContext.Object, _mapper);

            // Act
            var result = await roomsController.GetRoomsInHotel(hotelId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
            var notFoundResult = (NotFoundObjectResult)result.Result;
            Assert.Equal("No rooms found", notFoundResult.Value);
        }

        [Fact]
        public async Task GetRoomsInHotel_WhenHotelExistsWithRooms_ReturnsRooms()
        {
            // Arrange
            int hotelId = 1;
            var hotels = new List<Hotel> { DataGenerator.GenerateHotel(hotelId) }
                .AsQueryable();

            var rooms = new List<Room>
            {
                new Room
                {
                    Id = 1,
                    Capacity = 2,
                    Name = "Standard Room",
                    Description = "Cozy room with basic amenities",
                    Price = 100,
                    HotelId = hotelId
                },
                new Room
                {
                    Id = 2,
                    Capacity = 4,
                    Name = "Family Suite",
                    Description = "Spacious suite suitable for families",
                    Price = 200,
                    HotelId = hotelId
                },
                new Room
                {
                    Id = 3,
                    Capacity = 1,
                    Name = "Single Room",
                    Description = "Compact room for solo travelers",
                    Price = 80,
                    HotelId = hotelId
                },
            }.AsQueryable();

            var mockContext = new Mock<ApplicationDbContext>();

            var mockHotelSet = new Mock<DbSet<Hotel>>();
            mockHotelSet.SetupIQueryable(hotels);
            mockContext.SetupGet(m => m.Hotels).Returns(mockHotelSet.Object);

            var mockRoomSet = new Mock<DbSet<Room>>();
            mockRoomSet.SetupIQueryable(rooms);
            mockContext.SetupGet(m => m.Rooms).Returns(mockRoomSet.Object);

            var roomsController = new RoomsController(mockContext.Object, _mapper);

            // Act
            var result = await roomsController.GetRoomsInHotel(hotelId);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
            var okResult = (OkObjectResult)result.Result;
            var returnedRooms = (List<Room>)okResult.Value;
            Assert.Equal(3, returnedRooms.Count);
            Assert.Equal(1, returnedRooms[0].Id);
            Assert.Equal(2, returnedRooms[1].Id);
            Assert.Equal(3, returnedRooms[2].Id);
        }

        [Fact]
        public async Task PostRoom_WhenHotelExists_ReturnsCreatedResponse()
        {
            // Arrange
            int hotelId = 1;
            var hotels = new List<Hotel>
            {
                DataGenerator.GenerateHotel(hotelId)
            }.AsQueryable();
            var roomDto = new RoomDto
            {
                Capacity = 2,
                Name = "",
                Description = "",
                Price = 100,
                HotelId = hotelId
            };

            var rooms = new List<Room>().AsQueryable();

            var mockContext = new Mock<ApplicationDbContext>();

            var mockHotelSet = new Mock<DbSet<Hotel>>();
            mockHotelSet.SetupIQueryable(hotels);
            mockContext.SetupGet(m => m.Hotels).Returns(mockHotelSet.Object);

            var mockRoomSet = new Mock<DbSet<Room>>();
            mockRoomSet.SetupIQueryable(rooms);
            mockContext.SetupGet(m => m.Rooms).Returns(mockRoomSet.Object);

            var roomsController = new RoomsController(mockContext.Object, _mapper);

            // Act
            var result = await roomsController.PostRoom(roomDto);

            // Assert
            Assert.IsType<CreatedAtActionResult>(result.Result);
        }
    }
}