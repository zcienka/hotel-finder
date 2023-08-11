using AutoMapper;
using Backend.Controllers;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Backend.Tests.Systems;
using FakeItEasy;
using Backend.Data;

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
            var hotel = DataGenerator.GenerateHotel();
            string hotelId = hotel.Id;
            var hotels = new List<Hotel> { hotel }.AsQueryable();
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
            var result = await roomsController.GetAvailableRoomsInHotel(hotelId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
            var notFoundResult = (NotFoundObjectResult)result.Result;
            Assert.Equal("No rooms found", notFoundResult.Value);
        }

        [Fact]
        public async Task GetRoomsInHotel_WhenHotelExistsWithRooms_ReturnsRooms()
        {
            // Arrange
            var hotel = DataGenerator.GenerateHotel();
            string hotelId = hotel.Id;
            var hotels = new List<Hotel> { hotel }
                .AsQueryable();

            var rooms = new List<Room>
            {
                DataGenerator.GenerateRoom(hotelId, "1"),
                DataGenerator.GenerateRoom(hotelId, "2"),
                DataGenerator.GenerateRoom(hotelId, "3"),
            };

            var reservations = new List<Reservation>
            {
                DataGenerator.GenerateReservation(hotelId, "1", DateTime.Now, DateTime.MaxValue)
            };

            var mockContext = new Mock<ApplicationDbContext>();

            var mockHotelSet = new Mock<DbSet<Hotel>>();
            mockHotelSet.SetupIQueryable(hotels);
            mockContext.SetupGet(m => m.Hotels).Returns(mockHotelSet.Object);

            var mockRoomSet = new Mock<DbSet<Room>>();
            mockRoomSet.SetupIQueryable(rooms.AsQueryable());
            mockContext.SetupGet(m => m.Rooms).Returns(mockRoomSet.Object);

            var mockReservationSet = new Mock<DbSet<Reservation>>();
            mockReservationSet.SetupIQueryable(reservations.AsQueryable());
            mockContext.SetupGet(m => m.Reservations).Returns(mockReservationSet.Object);

            var roomsController = new RoomsController(mockContext.Object, _mapper);

            // Act
            var result = await roomsController.GetAvailableRoomsInHotel(hotelId);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
            var okResult = (OkObjectResult)result.Result;
            var returnedRooms = (List<Room>)okResult.Value;
            Assert.Equal(2, returnedRooms.Count);
        }

        [Fact]
        public async Task PostRoom_WhenHotelExists_ReturnsCreatedResponse()
        {
            // Arrange
            var hotel = DataGenerator.GenerateHotel();
            string hotelId = hotel.Id;

            var hotels = new List<Hotel> { hotel }.AsQueryable();
            string roomId = "1";
            var roomDto = DataGenerator.GenerateRoomDto(hotelId);

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

        [Fact]
        public async Task DeleteRoom_WhenRoomExistsWithReservation_DeletesRoomAndReservation()
        {
            // Arrange
            var hotel = DataGenerator.GenerateHotel();
            string hotelId = hotel.Id;
            string roomId = "1";

            var room = DataGenerator.GenerateRoom(hotelId, roomId);

            DateTime checkInDate = new DateTime(2023, 6, 6);
            DateTime checkOutDate = new DateTime(2023, 6, 8);

            var reservation = DataGenerator.GenerateReservation(hotelId, roomId, checkInDate, checkOutDate);

            string reservationId = reservation.Id;


            var rooms = new List<Room> { room }.AsQueryable();
            var reservations = new List<Reservation> { reservation }.AsQueryable();

            var mockContext = new Mock<ApplicationDbContext>();

            var mockRoomSet = new Mock<DbSet<Room>>();
            mockRoomSet.SetupIQueryable(rooms);
            mockContext.SetupGet(m => m.Rooms).Returns(mockRoomSet.Object);

            var mockReservationSet = new Mock<DbSet<Reservation>>();
            mockReservationSet.SetupIQueryable(reservations);
            mockContext.SetupGet(m => m.Reservations).Returns(mockReservationSet.Object);

            var roomsController = new RoomsController(mockContext.Object, _mapper);

            // Act
            var result = await roomsController.DeleteRoom(roomId);

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Null(await mockContext.Object.Rooms.FindAsync(roomId));
            Assert.Null(await mockContext.Object.Reservations.FindAsync(reservationId));
        }
    }
}