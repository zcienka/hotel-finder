using Backend.Core.IRepositories;
using Backend.Models;
using Moq;

namespace Backend.Tests.Systems.Mocks;

internal class MockIReservationRepository
{
    public static Mock<IReservationRepository> GetMock(string hotelId)
    {
        var mock = new Mock<IReservationRepository>();

        var reservations = new List<Reservation>
        {
            DataGenerator.GenerateReservation(hotelId, "1", DateTime.Now, DateTime.MaxValue)
        };

        mock.Setup(m => m.GetAll()).ReturnsAsync(() => reservations);
        return mock;
    }
}