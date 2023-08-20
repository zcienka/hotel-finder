using Backend.Core.IRepositories;
using Backend.Models;
using Moq;

namespace Backend.Tests.Systems.Mocks;

internal class MockIHotelRepository
{
    public static Mock<IHotelRepository> GetMock(string id)
    {
        var mock = new Mock<IHotelRepository>();
        var hotel = DataGenerator.GenerateHotel(id);
        var hotels = new List<Hotel>
        {
            hotel
        };

        mock.Setup(m => m.GetAll()).ReturnsAsync(() => hotels);
        mock.Setup(m => m.Exists(id)).ReturnsAsync(() => true);
        return mock;
    }
}