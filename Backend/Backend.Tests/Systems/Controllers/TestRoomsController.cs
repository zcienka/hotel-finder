using AutoMapper;
using Backend.Controllers;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Backend.Tests.Systems;
using FakeItEasy;
using Backend.Data;
using Backend.Interfaces;
using Backend.Repository;

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
        public async Task GetAvailableRoomsInHotel_ReturnsAvailableRooms()
        {
            // Arrange
            string hotelId = "1";

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

            var mockReservationRepository = new Mock<IReservationRepository>();
            mockReservationRepository.Setup(repo => repo.GetReservationsForHotel(It.IsAny<string>()))
                .Returns(reservations);

            var mockContext = new Mock<ApplicationDbContext>();

            var mockRoomSet = new Mock<DbSet<Room>>();
            mockRoomSet.SetupIQueryable(rooms.AsQueryable());
            mockContext.SetupGet(m => m.Rooms).Returns(mockRoomSet.Object);

            var roomRepository = new RoomRepository(mockContext.Object, mockReservationRepository.Object);

            // Act
            var result = roomRepository.GetAvailableRoomsById(hotelId);

            // Assert
            var returnedRooms = result;
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
            var room = DataGenerator.GenerateRoom(hotelId, roomId);

            var rooms = new List<Room>
            {
                room
            };
            var roomRequest = DataGenerator.GenerateRoomRequest(hotelId);
            var mockReservationRepository = new Mock<IReservationRepository>();

            var mockContext = new Mock<ApplicationDbContext>();
            var mockRoomSet = new Mock<DbSet<Room>>();
            mockRoomSet.SetupIQueryable(rooms.AsQueryable());
            mockContext.SetupGet(m => m.Rooms).Returns(mockRoomSet.Object);

            var mockHotelSet = new Mock<DbSet<Hotel>>();
            mockHotelSet.SetupIQueryable(hotels);
            mockContext.SetupGet(m => m.Hotels).Returns(mockHotelSet.Object);

            var roomRepository = new RoomRepository(mockContext.Object, mockReservationRepository.Object);
            var roomsController = new RoomsController(roomRepository, _mapper);

            // Act
            var result = await roomsController.PostRoom(roomRequest);

            // Assert
            Assert.IsType<CreatedAtActionResult>(result.Result);
        }
        
        [Fact]
        public async Task DeleteRoom_WhenRoomExists_DeletesRoom()
        {
            // Arrange
            var hotel = DataGenerator.GenerateHotel();
            string hotelId = hotel.Id;
            string roomId = "1";
        
            var room = DataGenerator.GenerateRoom(hotelId, roomId);
        
            var mockRoomRepository = new Mock<IRoomRepository>();

            var mockContext = new Mock<ApplicationDbContext>();
        
            var mockReservationSet = new Mock<DbSet<Reservation>>();
            mockContext.SetupGet(m => m.Reservations).Returns(mockReservationSet.Object);

            mockRoomRepository
                .Setup(repo => repo.GetByIdAsync(roomId))
                .ReturnsAsync(room);    

            var roomsController = new RoomsController(mockRoomRepository.Object, _mapper);

            // Act
            var result = await roomsController.DeleteRoom(roomId);
        
            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}