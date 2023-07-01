using Backend.Models;
using Bogus;

namespace Backend.Tests.Systems
{
    internal class DataGenerator
    {
        public static IQueryable<Hotel> GenerateHotels()
        {
            var hotels = new List<Hotel>();

            var hotelFaker = new Faker<Hotel>()
                .RuleFor(h => h.Name, f => f.Name.ToString())
                .RuleFor(h => h.Description, f => f.Lorem.Sentence())
                .RuleFor(h => h.Address, f => f.Address.StreetAddress())
                .RuleFor(h => h.City, f => f.Address.City())
                .RuleFor(h => h.PhoneNumber, f => f.Phone.PhoneNumber())
                .RuleFor(h => h.Stars, f => f.Random.Int(1, 5));

            hotels.Add(hotelFaker.Generate());

            return hotels.AsQueryable();
        }

        public static RoomDto GenerateRoomDto(string hotelId)
        {
            var roomFaker = new Faker<RoomDto>()
                .RuleFor(r => r.Capacity, f => f.Random.Int(1, 10))
                .RuleFor(r => r.Name, f => f.Company.CompanyName())
                .RuleFor(r => r.Description, f => f.Lorem.Sentence())
                .RuleFor(r => r.Price, f => f.Random.Int(50, 500))
                .RuleFor(r => r.HotelId, hotelId);

            return roomFaker.Generate();
        }
        public static Room GenerateRoom(string hotelId)
        {
            var roomFaker = new Faker<Room>()
                .RuleFor(r => r.Capacity, f => f.Random.Int(1, 10))
                .RuleFor(r => r.Name, f => f.Lorem.Sentence())
                .RuleFor(r => r.Description, f => f.Lorem.Sentence())
                .RuleFor(r => r.Price, f => f.Random.Int(50, 500))
                .RuleFor(r => r.HotelId, hotelId);

            return roomFaker.Generate();
        }

        public static Hotel GenerateHotel()
        {
            var hotelFaker = new Faker<Hotel>()
                .RuleFor(h => h.Name, f => f.Company.CompanyName())
                .RuleFor(h => h.Description, f => f.Lorem.Sentence())
                .RuleFor(h => h.Address, f => f.Address.StreetAddress())
                .RuleFor(h => h.City, f => f.Address.City())
                .RuleFor(h => h.PhoneNumber, f => f.Phone.PhoneNumber())
                .RuleFor(h => h.Stars, f => f.Random.Int(1, 5));

            return hotelFaker.Generate();
        }


        public static ReservationDto GenerateReservationDto(string hotelId, string roomId)
        {
            var reservationDto = new Faker<ReservationDto>()
                .RuleFor(r => r.CheckInDate, f => f.Date.Future())
                .RuleFor(r => r.CheckOutDate, f => f.Date.Future())
                .RuleFor(r => r.HotelId, f => hotelId)
                .RuleFor(r => r.RoomId, f => roomId)
                .RuleFor(r => r.UserId, f => f.Random.Int(1, 100).ToString())
                .Generate();

            return reservationDto;
        }

        public static Reservation GenerateReservation(string hotelId, string roomId)
        {
            var reservation = new Faker<Reservation>()
                .RuleFor(r => r.CheckInDate, f => f.Date.Future())
                .RuleFor(r => r.CheckOutDate, f => f.Date.Future())
                .RuleFor(r => r.HotelId, f => hotelId)
                .RuleFor(r => r.RoomId, roomId)
                .RuleFor(r => r.UserId, f => f.Random.Int(1, 100).ToString())
                .Generate();

            return reservation;
        }

        public static Comment GenerateComment(string userId, string hotelId)
        {
            var commentFaker = new Faker<Comment>()
                .RuleFor(c => c.Description, f => f.Lorem.Sentence())
                .RuleFor(c => c.UserId, f => userId)
                .RuleFor(c => c.HotelId, f => hotelId);

            return commentFaker.Generate();
        }
    }
}
