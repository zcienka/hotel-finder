// using AutoMapper;
// using Backend.Controllers;
// using Backend.Models;
// using FakeItEasy;
// using Microsoft.AspNetCore.Mvc;
// using Moq;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;
//
// namespace Backend.Tests.Systems.Controllers
// {
//     internal class TestReservationsController
//     {
//         private readonly IMapper _mapper;
//
//         public TestReservationsController()
//         {
//             _mapper = A.Fake<IMapper>();
//         }
//
//         [Fact]
//         public async Task PostReservation_WhenHotelDoesNotExist_ReturnsNotFoundResponse()
//         {
//             // Arrange
//             var reservationDto = new ReservationDto
//             {
//                 RoomId = 1,
//             };
//
//             var mapperMock = new Mock<IMapper>();
//
//             var contextMock = new Mock<ApplicationDbContext>();
//             contextMock.Setup(c => c.Reservations.ToList()).Returns(new List<Reservation> { new Reservation() });
//             contextMock.Setup(c => c.Hotels.FirstOrDefault(It.IsAny<Func<Hotel, bool>>())).Returns((Hotel)null);
//
//             var reservationsController = new ReservationsController(contextMock.Object, mapperMock.Object);
//
//             // Act
//             var result = await reservationsController.PostReservation(reservationDto);
//
//             // Assert
//             Assert.IsType<NotFoundObjectResult>(result.Result);
//             var notFoundResult = (NotFoundObjectResult)result.Result;
//             Assert.Equal("Hotel not found", notFoundResult.Value);
//         }
//     }
// }
