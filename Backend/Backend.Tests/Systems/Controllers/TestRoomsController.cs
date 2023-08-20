using AutoMapper;
using Backend.Controllers;
using Backend.Core.IConfiguration;
using Backend.Models;
using Moq;
using Backend.Tests.Systems;
using FakeItEasy;
using Backend.Data;
using Backend.Tests.Systems.Mocks;
using Microsoft.AspNetCore.Mvc;
using Backend.Dtos;
using Backend.Requests;

namespace Backend.Tests.Controllers;

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

        var mock = new Mock<IUnitOfWork>();

        var availableRooms = new List<Room>
        {
            DataGenerator.GenerateRoom(hotelId, "2"),
            DataGenerator.GenerateRoom(hotelId, "3"),
        }.AsEnumerable();

        var roomRepoMock = MockIRoomRepository.GetMock(hotelId, availableRooms);
        var reservationRepoMock = MockIReservationRepository.GetMock(hotelId);
        var hotelsRepoMock = MockIHotelRepository.GetMock(hotelId);

        mock.Setup(m => m.Rooms).Returns(() => roomRepoMock.Object);
        mock.Setup(m => m.Reservations).Returns(() => reservationRepoMock.Object);
        mock.Setup(m => m.Hotels).Returns(() => hotelsRepoMock.Object);
        mock.Setup(m => m.CompleteAsync()).Callback(() => { return; });

        var roomsController = new RoomsController(mock.Object, _mapper);

        // Act
        var result = await roomsController.GetAvailableRoomsInHotel(hotelId, new PagingQuery());

        // Assert
        Assert.IsType<ActionResult<ApiResult<RoomDto>>>(result);
        var apiResult = result.Value.Count;
        Assert.Equal(2, apiResult);
    }

    [Fact]
    public async Task GetAvailableRoomsInHotel_HotelNotFound_ReturnsNotFound()
    {
        // Arrange
        string hotelId = "1";
        var mock = new Mock<IUnitOfWork>();
        mock.Setup(uow => uow.Hotels.Exists(hotelId)).ReturnsAsync(false);

        var roomsController = new RoomsController(mock.Object, _mapper);

        // Act
        var result = await roomsController.GetAvailableRoomsInHotel(hotelId, new PagingQuery());

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
        var notFoundResult = result.Result as NotFoundObjectResult;
        Assert.Equal("Hotel with given id does not exist", notFoundResult.Value);
    }

    [Fact]
    public async Task GetRooms_ValidRequest_ReturnsRooms()
    {
        // Arrange
        var mock = new Mock<IUnitOfWork>();
        string hotelId = "1";
        var availableRooms = new List<Room>
        {
            DataGenerator.GenerateRoom(hotelId, "2"),
            DataGenerator.GenerateRoom(hotelId, "3"),
        }.AsEnumerable();

        mock.Setup(uow => uow.Rooms.GetAll()).ReturnsAsync(availableRooms);
        var roomsController = new RoomsController(mock.Object, _mapper);

        // Act
        var result = await roomsController.GetRooms(new PagingQuery { Limit = "10", Offset = "0" });

        // Assert
        Assert.IsType<ActionResult<ApiResult<Room>>>(result);

        var apiResult = result.Value.Count;
        Assert.Equal(2, apiResult);
    }

    [Fact]
    public void GetRoom_RoomNotFound_ReturnsNotFound()
    {
        // Arrange
        string roomId = "1";
        var mock = new Mock<IUnitOfWork>();
        mock.Setup(uow => uow.Rooms.Exists(roomId)).ReturnsAsync(false);
        var roomsController = new RoomsController(mock.Object, _mapper);

        // Act
        var result = roomsController.GetRoom(roomId);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result.Result);
    }

    [Fact]
    public void GetRoom_RoomFound_ReturnsRoomDto()
    {
        // Arrange
        string roomId = "1";
        string hotelId = "1";
        var room = DataGenerator.GenerateRoom(hotelId, roomId);

        var mock = new Mock<IUnitOfWork>();
        mock.Setup(uow => uow.Rooms.GetById(roomId)).ReturnsAsync(room);
        mock.Setup(uow => uow.Rooms.Exists(roomId)).ReturnsAsync(true);
        var roomsController = new RoomsController(mock.Object, _mapper);

        // Act
        var result = roomsController.GetRoom(roomId);

        // Assert
        Assert.IsType<ActionResult<RoomDto>>(result.Result);
        var roomDto = result.Result.Value;
        Assert.NotNull(roomDto);
    }

    [Fact]
    public async Task PutRoom_ValidUpdate_ReturnsNoContent()
    {
        // Arrange
        string roomId = "1";
        string hotelId = "1";
        var roomRequest = DataGenerator.GenerateRoomRequest(hotelId, roomId);
        var room = DataGenerator.GenerateRoom(hotelId, roomId);

        var mock = new Mock<IUnitOfWork>();
        mock.Setup(uow => uow.Hotels.Exists(roomRequest.HotelId)).ReturnsAsync(true);
        mock.Setup(uow => uow.Rooms.Update(room)).Returns(true);
        mock.Setup(uow => uow.Rooms.Exists(roomId)).ReturnsAsync(true);
        var roomsController = new RoomsController(mock.Object, _mapper);

        // Act
        var result = await roomsController.PutRoom(roomId, roomRequest);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task PutRoom_InvalidId_ReturnsBadRequest()
    {
        // Arrange
        string roomId = "1";
        string hotelId = "1";
        var mock = new Mock<IUnitOfWork>();

        var roomRequest = DataGenerator.GenerateRoomRequest(hotelId, roomId);
        var roomsController = new RoomsController(mock.Object, _mapper);

        // Act
        var result = await roomsController.PutRoom("2", roomRequest);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task PutRoom_HotelNotFound_ReturnsNotFound()
    {
        // Arrange
        string roomId = "1";
        string hotelId = "1";
        var roomRequest = DataGenerator.GenerateRoomRequest(hotelId, roomId);
        var mock = new Mock<IUnitOfWork>();

        var roomsController = new RoomsController(mock.Object, _mapper);

        mock.Setup(uow => uow.Hotels.Exists(roomRequest.HotelId)).ReturnsAsync(false);

        // Act
        var result = await roomsController.PutRoom(roomId, roomRequest);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Hotel not found", notFoundResult.Value);
    }


    [Fact]
    public async Task PostRoom_ValidRoom_ReturnsCreatedResponse()
    {
        // Arrange
        string roomId = "1";
        string hotelId = "1";
        var roomRequest = DataGenerator.GenerateRoomRequest(hotelId, roomId);
        var room = DataGenerator.GenerateRoom(hotelId, roomId);
        var mock = new Mock<IUnitOfWork>();
        var roomsController = new RoomsController(mock.Object, _mapper);

        mock.Setup(uow => uow.Hotels.Exists(roomRequest.HotelId)).ReturnsAsync(true);
        mock.Setup(uow => uow.Rooms.Add(room)).ReturnsAsync(true);

        // Act
        var result = await roomsController.PostRoom(roomRequest);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(nameof(roomsController.GetRoom), createdAtActionResult.ActionName);
    }


    [Fact]
    public async Task PostRoom_HotelNotFound_ReturnsNotFound()
    {
        // Arrange
        string roomId = "1";
        string hotelId = "1";
        var roomRequest = DataGenerator.GenerateRoomRequest(hotelId, roomId);
        var mock = new Mock<IUnitOfWork>();
        var roomsController = new RoomsController(mock.Object, _mapper);

        mock.Setup(uow => uow.Hotels.Exists(roomRequest.HotelId)).ReturnsAsync(false);

        // Act
        var result = await roomsController.PostRoom(roomRequest);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal("Hotel not found", notFoundResult.Value);
    }

    [Fact]
    public async Task DeleteRoom_RoomFound_ReturnsNoContent()
    {
        // Arrange
        string roomId = "1";
        string hotelId = "1";

        var roomEntity = DataGenerator.GenerateRoom(hotelId, roomId);
        var mock = new Mock<IUnitOfWork>();
        var roomsController = new RoomsController(mock.Object, _mapper);

        mock.Setup(uow => uow.Rooms.Exists(roomId)).ReturnsAsync(true);

        // Act
        var result = await roomsController.DeleteRoom(roomId);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteRoom_RoomNotFound_ReturnsNotFound()
    {
        // Arrange
        string roomId = "1";
        string hotelId = "1";

        var mock = new Mock<IUnitOfWork>();
        var roomsController = new RoomsController(mock.Object, _mapper);

        mock.Setup(uow => uow.Rooms.Exists(roomId)).ReturnsAsync(false);

        // Act
        var result = await roomsController.DeleteRoom(roomId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
