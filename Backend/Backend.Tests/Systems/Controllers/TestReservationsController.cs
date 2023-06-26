using AutoMapper;
using Backend.Controllers;
using Backend.Models;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Backend.Tests.Systems.Controllers
{
    public class TestReservationsController
    {
        private readonly IMapper _mapper;

        public TestReservationsController()
        {
            _mapper = A.Fake<IMapper>();
        }

        [Fact]
        public async Task PostReservation_WhenHotelDoesNotExist_ReturnsNotFoundResponse()
        {
            // Arrange
            int roomId = 1;
            int hotelId = 1;
            var reservationDto = DataGenerator.GenerateReservationDto(hotelId, roomId);

            var mockContext = new Mock<ApplicationDbContext>();

            var mockReservationSet = new Mock<DbSet<Reservation>>();
            var mockHotelSet = new Mock<DbSet<Hotel>>();

            var hotels = new List<Hotel>().AsQueryable();
            var reservations = new List<Reservation>().AsQueryable();

            mockReservationSet.SetupIQueryable(reservations);
            mockHotelSet.SetupIQueryable(hotels);

            mockContext.Setup(c => c.Reservations).Returns(mockReservationSet.Object);
            mockContext.Setup(c => c.Hotels).Returns(mockHotelSet.Object);

            var reservationsController = new ReservationsController(mockContext.Object, _mapper);

            // Act
            var result = await reservationsController.PostReservation(reservationDto);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
            var notFoundResult = (NotFoundObjectResult)result.Result;
            Assert.Equal("Hotel not found", notFoundResult.Value);
        }

        [Fact]
        public async Task PostReservation_WhenRoomDoesNotExist_ReturnsNotFoundResponse()
        {
            // Arrange
            int hotelId = 1;
            int roomId = 1;
            var reservationDto = DataGenerator.GenerateReservationDto(hotelId, roomId);

            var mockContext = new Mock<ApplicationDbContext>();

            var mockHotelSet = new Mock<DbSet<Hotel>>();
            var mockRoomSet = new Mock<DbSet<Room>>();

            var hotel = DataGenerator.GenerateHotel(hotelId);

            var hotels = new List<Hotel> { hotel }.AsQueryable();
            var rooms = new List<Room> { }.AsQueryable();

            mockHotelSet.SetupIQueryable(hotels);
            mockRoomSet.SetupIQueryable(rooms);

            mockContext.Setup(c => c.Hotels).Returns(mockHotelSet.Object);
            mockContext.Setup(c => c.Rooms).Returns(mockRoomSet.Object);

            var reservationsController = new ReservationsController(mockContext.Object, _mapper);

            // Act
            var result = await reservationsController.PostReservation(reservationDto);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
            var notFoundResult = (NotFoundObjectResult)result.Result;
            Assert.Equal("Room not found", notFoundResult.Value);
        }


        [Fact]
        public async Task PostReservation_WhenHotelExists_ReturnsReservation()
        {
            // Arrange
            int roomId = 1;
            int hotelId = 2;
            var reservationDto = DataGenerator.GenerateReservationDto(hotelId, roomId);

            var mockContext = new Mock<ApplicationDbContext>();

            var mockReservationSet = new Mock<DbSet<Reservation>>();
            var mockHotelSet = new Mock<DbSet<Hotel>>();
            var mockRoomSet = new Mock<DbSet<Room>>();

            var hotel = DataGenerator.GenerateHotel(hotelId);
            var room = DataGenerator.GenerateRoom(roomId, hotelId);

            var hotels = new List<Hotel> { hotel }.AsQueryable();
            var reservations = new List<Reservation>()
            {
                DataGenerator.GenerateReservation(1, hotelId, roomId),
                DataGenerator.GenerateReservation(2, hotelId, roomId)
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

            mockReservationSet.SetupIQueryable(reservations);
            mockHotelSet.SetupIQueryable(hotels);

            mockContext.SetupGet(e => e.Reservations).Returns(mockReservationSet.Object);
            mockContext.SetupGet(c => c.Hotels).Returns(mockHotelSet.Object);
            mockRoomSet.SetupIQueryable(rooms);
            mockContext.SetupGet(c => c.Rooms).Returns(mockRoomSet.Object);

            var reservationsController = new ReservationsController(mockContext.Object, _mapper);

            // Act
            var result = await reservationsController.PostReservation(reservationDto);

            // Assert
            Assert.IsType<CreatedAtActionResult>(result.Result);
        }

        [Fact]
        public async Task PostReservation_WhenReservationDoesNotConflict_ReturnsCreated()
        {
            // Arrange
            int hotelId = 1;
            int roomId = 2;

            var reservationDto = new ReservationDto
            {
                HotelId = hotelId,
                UserId = 1,
                RoomId = roomId,
                CheckInDate = new DateTime(2023, 6, 1),
                CheckOutDate = new DateTime(2023, 6, 5)
            };

            var existingReservation = new Reservation
            {
                HotelId = hotelId,
                UserId = 1,
                RoomId = roomId,
                CheckInDate = new DateTime(2023, 6, 6),
                CheckOutDate = new DateTime(2023, 6, 8)
            };

            var mockContext = new Mock<ApplicationDbContext>();

            var mockReservationSet = new Mock<DbSet<Reservation>>();
            var mockHotelSet = new Mock<DbSet<Hotel>>();
            var mockRoomSet = new Mock<DbSet<Room>>();

            var hotel = DataGenerator.GenerateHotel(hotelId);
            var room = DataGenerator.GenerateRoom(roomId, hotelId);

            var hotels = new List<Hotel> { hotel }.AsQueryable();
            var reservations = new List<Reservation> { existingReservation }.AsQueryable();
            var rooms = new List<Room>{ room }.AsQueryable();

            mockReservationSet.SetupIQueryable(reservations);
            mockHotelSet.SetupIQueryable(hotels);
            mockRoomSet.SetupIQueryable(rooms);

            mockContext.Setup(c => c.Reservations).Returns(mockReservationSet.Object);
            mockContext.Setup(c => c.Hotels).Returns(mockHotelSet.Object);
            mockContext.Setup(c => c.Rooms).Returns(mockRoomSet.Object);

            var reservationController = new ReservationsController(mockContext.Object, _mapper);

            // Act
            var result = await reservationController.PostReservation(reservationDto);

            // Assert
            Assert.IsType<CreatedAtActionResult>(result.Result);
        }
    }
}