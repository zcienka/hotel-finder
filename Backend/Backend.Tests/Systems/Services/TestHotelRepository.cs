using AutoMapper;
using Backend.Data;
using Backend.Models;
using Backend.Repository;
using Moq;
using Microsoft.EntityFrameworkCore;

namespace Backend.Tests.Systems.Services
{
    public class TestHotelRepository
    {

        [Fact]
        public void GetSearchResults_WithNameFilter_ShouldReturnFilteredHotels()
        {
            // Arrange
            var name = "Hotel A";

            var mockContext = new Mock<ApplicationDbContext>();

            var mockHotelSet = new Mock<DbSet<Hotel>>();

            int userId = 2;
            int commentId = 3;

            var hotels = new List<Hotel>
            {
                new()
                {
                    Id = "1", Name = name, City = "City A", Category = "Category A", Description = "", Address = "",
                    PhoneNumber = ""
                },
                new()
                {
                    Id = "2", Name = "Hotel B", City = "City B", Category = "Category B", Description = "",
                    Address = "", PhoneNumber = ""
                },
            };


            mockHotelSet.SetupIQueryable(hotels.AsQueryable());

            mockContext.Setup(c => c.Hotels).Returns(mockHotelSet.Object);


            var hotelRepository = new HotelRepository(mockContext.Object);

            // Act
            var result = hotelRepository.GetSearchResults(name, null, null, null, null, null);

            // Assert
            Assert.Single(result);
            Assert.Equal(hotels[0], result.First());
        }

        [Fact]
        public void GetSearchResults_WithCategoryFilter_ShouldReturnFilteredHotels()
        {
            // Arrange
            var category = "Category A";

            var mockContext = new Mock<ApplicationDbContext>();

            var mockHotelSet = new Mock<DbSet<Hotel>>();

            int userId = 2;
            int commentId = 3;

            var hotels = new List<Hotel>
            {
                new()
                {
                    Id = "1", Name = "Hotel A", City = "City A", Category = category, Description = "", Address = "",
                    PhoneNumber = ""
                },
                new()
                {
                    Id = "2", Name = "Hotel B", City = "City B", Category = "Category B", Description = "",
                    Address = "", PhoneNumber = ""
                },
            };


            mockHotelSet.SetupIQueryable(hotels.AsQueryable());

            mockContext.Setup(c => c.Hotels).Returns(mockHotelSet.Object);


            var hotelRepository = new HotelRepository(mockContext.Object);

            // Act
            var result = hotelRepository.GetSearchResults(null, null, null, null, null, category);

            // Assert
            Assert.Single(result);
            Assert.Equal(hotels[0], result.First());
        }


        [Fact]
        public void GetSearchResults_WithCityFilter_ShouldReturnFilteredHotels()
        {
            // Arrange
            var city = "City A";

            var mockContext = new Mock<ApplicationDbContext>();

            var mockHotelSet = new Mock<DbSet<Hotel>>();

            int userId = 2;
            int commentId = 3;

            var hotels = new List<Hotel>
            {
                new()
                {
                    Id = "1", Name = "Hotel A", City = city, Category = "Category A", Description = "", Address = "",
                    PhoneNumber = ""
                },
                new()
                {
                    Id = "2", Name = "Hotel B", City = "City B", Category = "Category B", Description = "",
                    Address = "", PhoneNumber = ""
                },
            };


            mockHotelSet.SetupIQueryable(hotels.AsQueryable());

            mockContext.Setup(c => c.Hotels).Returns(mockHotelSet.Object);

            var hotelRepository = new HotelRepository(mockContext.Object);

            // Act
            var result = hotelRepository.GetSearchResults(null, city, null, null, null, null);

            // Assert
            Assert.Single(result);
            Assert.Equal(hotels[0], result.First());
        }
        [Fact]
        public void GetSearchResults_WithRoomCountFilter_ShouldReturnFilteredHotels()
        {
            // Arrange
            var hotelId = "1";

            var mockContext = new Mock<ApplicationDbContext>();

            var mockHotelSet = new Mock<DbSet<Hotel>>();
            var mockRoomSet = new Mock<DbSet<Room>>();

            int userId = 2;
            int commentId = 3;

            var hotels = new List<Hotel>
            {
                new()
                {
                    Id = hotelId, Name = "Hotel A", City = "City A", Category = "Category A", Description = "", Address = "",
                    PhoneNumber = ""
                },
                new()
                {
                    Id = "2", Name = "Hotel B", City = "City B", Category = "Category B", Description = "",
                    Address = "", PhoneNumber = ""
                },
            };
            var rooms = new List<Room>
            {
                DataGenerator.GenerateRoom(hotelId, "1"),
                DataGenerator.GenerateRoom(hotelId, "2"),
                DataGenerator.GenerateRoom(hotelId, "3"),
            }.AsQueryable();

            mockHotelSet.SetupIQueryable(hotels.AsQueryable());
            mockRoomSet.SetupIQueryable(rooms);

            mockContext.Setup(c => c.Hotels).Returns(mockHotelSet.Object);
            mockContext.Setup(c => c.Rooms).Returns(mockRoomSet.Object);

            var hotelRepository = new HotelRepository(mockContext.Object);

            // Act
            var result = hotelRepository.GetSearchResults(null, null, null, null, 3, null);

            // Assert
            Assert.Single(result);
            Assert.Equal(hotels[0], result.First());
        }

        [Fact]
        public void GetSearchResults_WithCheckInCheckOutDateFilter_ShouldReturnFilteredHotels()
        {
            // Arrange
            var checkInDate = new DateTime(2023, 6, 2);
            var checkOutDate = new DateTime(2023, 6, 4);

            var mockContext = new Mock<ApplicationDbContext>();
            var mockHotelSet = new Mock<DbSet<Hotel>>();
            var mockReservationSet = new Mock<DbSet<Reservation>>();
            var mockRoomSet = new Mock<DbSet<Room>>();
            var hotelId = "1";
            var roomId = "1";

            int userId = 2;
            int commentId = 3;

            var hotels = new List<Hotel>
            {
                new()
                {
                    Id = hotelId, Name = "Hotel A", City = "City A", Category = "Category A", Description = "", Address = "",
                    PhoneNumber = ""
                },
                new()
                {
                    Id = "2", Name = "Hotel B", City = "City B", Category = "Category B", Description = "",
                    Address = "", PhoneNumber = ""
                },
            };


            var reservations = new List<Reservation>()
            {
                DataGenerator.GenerateReservation(hotelId, roomId, new DateTime(2023, 6, 6), new DateTime(2023, 6, 8)),
                DataGenerator.GenerateReservation(hotelId, roomId, new DateTime(2023, 6, 9), new DateTime(2023, 6, 10))
            }.AsQueryable();

            var rooms = new List<Room>
            {
                DataGenerator.GenerateRoom(hotelId, roomId)
            }.AsQueryable();


            mockHotelSet.SetupIQueryable(hotels.AsQueryable());
            mockReservationSet.SetupIQueryable(reservations);
            mockRoomSet.SetupIQueryable(rooms);

            mockContext.Setup(c => c.Hotels).Returns(mockHotelSet.Object);
            mockContext.Setup(c => c.Reservations).Returns(mockReservationSet.Object);
            mockContext.Setup(c => c.Rooms).Returns(mockRoomSet.Object);

            var hotelRepository = new HotelRepository(mockContext.Object);

            // Act
            var result = hotelRepository.GetSearchResults(null, null, checkInDate, checkOutDate, null, null);

            // Assert
            Assert.Single(result);
            Assert.Equal(hotels[0], result.First());
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllHotelsOrderedDescendingById()
        {
            // Arrange
            var hotels = new List<Hotel>
            {
                new()  { Id = "1", Name = "Hotel A",City = "City A", Category = "Category A", Description = "", Address = "",
                    PhoneNumber = "" },
                new()  { Id = "2", Name = "Hotel B",City = "City B", Category = "Category B", Description = "",
                    Address = "", PhoneNumber = "" },
                new()  { Id = "3", Name = "Hotel C",City = "City B", Category = "Category B", Description = "",
                    Address = "", PhoneNumber = "" }
            };

            var mockHotelSet = new Mock<DbSet<Hotel>>();
            mockHotelSet.SetupIQueryable(hotels.AsQueryable());


            var mockContext = new Mock<ApplicationDbContext>();
            mockContext.Setup(c => c.Hotels).Returns(mockHotelSet.Object);

            var hotelRepository = new HotelRepository(mockContext.Object);

            // Act
            var result = hotelRepository.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
            
            var orderedHotels = result.OrderByDescending(h => h.Id).ToList();
            Assert.Equal("3", orderedHotels[0].Id);
            Assert.Equal("2", orderedHotels[1].Id);
            Assert.Equal("1", orderedHotels[2].Id);
        }
    }
}