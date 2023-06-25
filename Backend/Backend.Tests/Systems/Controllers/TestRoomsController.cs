using AutoMapper;
using Backend.Controllers;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Backend.Tests.Systems;

namespace Backend.Tests.Controllers
{
    public class TestRoomsController
    {
        private readonly IMapper _mapper;

        public TestRoomsController()
        {
            _mapper = Mock.Of<IMapper>();
        }

        [Fact]
        public async Task GetRoomsInHotel_WhenHotelExistsButNoRooms_ReturnsNotFound()
        {
            // Arrange
            int hotelId = 1;
            var hotels = new List<Hotel>
            {
                new Hotel
                {
                    Id = hotelId,
                    Name = "Hotel 1",
                    Description = "Description 1",
                    Address = "Address 1",
                    City = "City 1",
                    PhoneNumber = "123456789",
                    Stars = 3
                }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Hotel>>();
            mockSet.SetupIQueryable(hotels);

            var mockContext = new Mock<ApplicationDbContext>();
            mockContext.SetupGet(m => m.Hotels).Returns(mockSet.Object);

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
            var hotels = new List<Hotel>
            {
                new Hotel
                {
                    Id = hotelId,
                    Name = "Hotel 1",
                    Description = "Description 1",
                    Address = "Address 1",
                    City = "City 1",
                    PhoneNumber = "123456789",
                    Stars = 3
                }
            }.AsQueryable();

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

            var mockSet = new Mock<DbSet<Hotel>>();
            mockSet.SetupIQueryable(hotels);
            mockContext.SetupGet(m => m.Hotels).Returns(mockSet.Object);


            var roomSet = new Mock<DbSet<Room>>();
            roomSet.SetupIQueryable(rooms);
            mockContext.SetupGet(m => m.Rooms).Returns(roomSet.Object);

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
    }
}
