using AutoMapper;
using Backend.Controllers;
using Backend.Data;
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
            var hotel = DataGenerator.GenerateHotel();
            string hotelId = hotel.Id;

            var reservationDto = DataGenerator.GenerateReservationDto(hotelId, "roomId", DateTime.MinValue, DateTime.MaxValue);

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
            var hotel = DataGenerator.GenerateHotel();
            string hotelId = hotel.Id;

            var reservationDto = DataGenerator.GenerateReservationDto(hotelId, "roomId", DateTime.MinValue, DateTime.MaxValue);

            var mockContext = new Mock<ApplicationDbContext>();

            var mockHotelSet = new Mock<DbSet<Hotel>>();
            var mockRoomSet = new Mock<DbSet<Room>>();

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
            Assert.Equal("Room with that id not found", notFoundResult.Value);
        }


        [Fact]
        public async Task PostReservation_WhenHotelExists_ReturnsReservation()
        {
            // Arrange
            var hotel = DataGenerator.GenerateHotel();
            string hotelId = hotel.Id;
            string roomId = "1";
            var room = DataGenerator.GenerateRoom(hotelId, roomId);

            var reservationDto = DataGenerator.GenerateReservationDto(hotelId, roomId, DateTime.MinValue, DateTime.MaxValue);

            var mockContext = new Mock<ApplicationDbContext>();

            var mockReservationSet = new Mock<DbSet<Reservation>>();
            var mockHotelSet = new Mock<DbSet<Hotel>>();
            var mockRoomSet = new Mock<DbSet<Room>>();

            DateTime checkInDate = new DateTime(2023, 6, 6);
            DateTime checkOutDate = new DateTime(2023, 6, 8);

            var hotels = new List<Hotel> { hotel }.AsQueryable();
            var reservations = new List<Reservation>()
            {
                DataGenerator.GenerateReservation(hotelId, roomId, checkInDate, checkOutDate),
                DataGenerator.GenerateReservation(hotelId, roomId, checkInDate, checkOutDate)
            }.AsQueryable();
            var rooms = new List<Room>
            {
                DataGenerator.GenerateRoom(hotelId, roomId),
                DataGenerator.GenerateRoom(hotelId, roomId),
                DataGenerator.GenerateRoom(hotelId, roomId)
            }.AsQueryable();

            mockReservationSet.SetupIQueryable(reservations);
            mockHotelSet.SetupIQueryable(hotels);
            mockRoomSet.SetupIQueryable(rooms);

            mockContext.SetupGet(e => e.Reservations).Returns(mockReservationSet.Object);
            mockContext.SetupGet(c => c.Hotels).Returns(mockHotelSet.Object);
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
            var hotel = DataGenerator.GenerateHotel();
            string hotelId = hotel.Id;
            string roomId = "1";
            var room = DataGenerator.GenerateRoom(hotelId, roomId);

            DateTime checkInDate = new DateTime(2023, 6, 6);
            DateTime checkOutDate = new DateTime(2023, 6, 8);

            var reservation = DataGenerator.GenerateReservationDto(hotelId, roomId, checkInDate, checkOutDate);

            var existingReservation = DataGenerator.GenerateReservation(hotelId, roomId, checkInDate, checkOutDate);

            var mockContext = new Mock<ApplicationDbContext>();

            var mockReservationSet = new Mock<DbSet<Reservation>>();
            var mockHotelSet = new Mock<DbSet<Hotel>>();
            var mockRoomSet = new Mock<DbSet<Room>>();

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
            var result = await reservationController.PostReservation(reservation);

            // Assert
            Assert.IsType<CreatedAtActionResult>(result.Result);
        }

        [Fact]
        public async Task PostReservation_WhenReservationDoesConflict_ReturnsBadRequest()
        {
            // Arrange
            var hotel = DataGenerator.GenerateHotel();
            string hotelId = hotel.Id;
            string roomId = "1";
            var room = DataGenerator.GenerateRoom(hotelId, roomId);
            DateTime checkInDate = new DateTime(2023, 6, 6);
            DateTime checkOutDate = new DateTime(2023, 6, 8);

            var reservationDto = DataGenerator.GenerateReservationDto(hotelId, roomId, checkInDate, checkOutDate);

            var existingReservation = DataGenerator.GenerateReservation(hotelId, roomId, checkInDate, checkOutDate);

            var mockContext = new Mock<ApplicationDbContext>();

            var mockReservationSet = new Mock<DbSet<Reservation>>();
            var mockHotelSet = new Mock<DbSet<Hotel>>();
            var mockRoomSet = new Mock<DbSet<Room>>();

            var hotels = new List<Hotel> { hotel }.AsQueryable();
            var reservations = new List<Reservation> { existingReservation }.AsQueryable();
            var rooms = new List<Room> { room }.AsQueryable();

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
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }
    }
}