using Moq;
using Backend.Core.IRepositories;
using Backend.Models;

namespace Backend.Tests.Systems.Mocks;

internal class MockIRoomRepository
{
    public static Mock<IRoomRepository> GetMock(string hotelId, IEnumerable<Room> availableRooms = null)
    {
        var mock = new Mock<IRoomRepository>();

        var rooms = new List<Room>
        {
            DataGenerator.GenerateRoom(hotelId, "1"),
            DataGenerator.GenerateRoom(hotelId, "2"),
            DataGenerator.GenerateRoom(hotelId, "3"),
        }.AsEnumerable();

        mock.Setup(m => m.GetAll()).ReturnsAsync(() => rooms);
        mock.Setup(m => m.GetAvailableRoomsById(hotelId)).Returns(() => availableRooms);

        return mock;
    }
}