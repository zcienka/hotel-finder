using AutoMapper;
using Backend.Controllers;
using Backend.Data;
using Backend.Interfaces;
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
        public async Task PostReservation_WhenRoomDoesNotExist_ReturnsNotFoundResponse()
        {
            // Arrange
            var hotel = DataGenerator.GenerateHotel();
            string hotelId = hotel.Id;

            DateTime checkInDate0 = new DateTime(2023, 6, 4);
            DateTime checkOutDate0 = new DateTime(2023, 6, 5);
            string userEmail = "example@example.com";

            var reservationRequest =
                DataGenerator.GenerateReservationRequest(hotelId, "roomId", checkInDate0, checkOutDate0, userEmail);

            var mockReservationRepository = new Mock<IReservationRepository>();


            mockReservationRepository
                .Setup(repo => repo.HotelExists(hotelId))
                .Returns(true);

            var reservationsController = new ReservationsController(mockReservationRepository.Object, _mapper);

            // Act
            var result = await reservationsController.PostReservation(reservationRequest);

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
            string userEmail = "example@example.com";

            DateTime checkInDate0 = new DateTime(2023, 6, 4);
            DateTime checkOutDate0 = new DateTime(2023, 6, 5);

            var reservationRequest =
                DataGenerator.GenerateReservationRequest(hotelId, roomId, checkInDate0, checkOutDate0, userEmail);

            DateTime checkInDate1 = new DateTime(2023, 6, 6);
            DateTime checkOutDate1 = new DateTime(2023, 6, 8);

            DateTime checkInDate2 = new DateTime(2023, 6, 9);
            DateTime checkOutDate2 = new DateTime(2023, 6, 10);

            var reservations = new List<Reservation>()
            {
                DataGenerator.GenerateReservation(hotelId, roomId, checkInDate1, checkOutDate1),
                DataGenerator.GenerateReservation(hotelId, roomId, checkInDate2, checkOutDate2)
            }.AsQueryable();

            var mockReservationRepository = new Mock<IReservationRepository>();

            mockReservationRepository
                .Setup(repo => repo.GetAll())
                .ReturnsAsync(reservations);

            mockReservationRepository
                .Setup(repo => repo.IsRoomInHotel(hotelId, roomId))
                .Returns(true);

            mockReservationRepository
                .Setup(repo => repo.HotelExists(hotelId))
                .Returns(true);

            mockReservationRepository
                .Setup(repo => repo.UserExists(userEmail))
                .Returns(true);

            var reservationsController = new ReservationsController(mockReservationRepository.Object, _mapper);

            // Act
            var result = await reservationsController.PostReservation(reservationRequest);

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
            string userEmail = "example@example.com";
            DateTime checkInDate = new DateTime(2023, 6, 6);
            DateTime checkOutDate = new DateTime(2023, 6, 8);

            var reservation = DataGenerator.GenerateReservationRequest(hotelId, roomId, checkInDate, checkOutDate, userEmail);

            var existingReservation = DataGenerator.GenerateReservation(hotelId, roomId, checkInDate, checkOutDate);

            var reservations = new List<Reservation> { existingReservation }.AsQueryable();

            var mockReservationRepository = new Mock<IReservationRepository>();

            mockReservationRepository
                .Setup(repo => repo.GetAll())
                .ReturnsAsync(reservations);

            mockReservationRepository
                .Setup(repo => repo.IsRoomInHotel(hotelId, roomId))
                .Returns(true);

            mockReservationRepository
                .Setup(repo => repo.HotelExists(hotelId))
                .Returns(true);

            mockReservationRepository
                .Setup(repo => repo.UserExists(userEmail))
                .Returns(true);


            var reservationController = new ReservationsController(mockReservationRepository.Object, _mapper);

            // Act
            var result = await reservationController.PostReservation(reservation);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);

        }
    }
}
