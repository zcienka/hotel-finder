﻿using Backend.Dtos;
using Backend.Models;
using Backend.Requests;
using Backend.Responses;
using Bogus;

namespace Backend.Tests.Systems;

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

    public static RoomRequest GenerateRoomRequest(string hotelId, string roomId = "")
    {
        if (roomId == "")
        {
            var roomFaker = new Faker<RoomRequest>()
                .RuleFor(r => r.Capacity, f => f.Random.Int(1, 10))
                .RuleFor(r => r.Name, f => f.Company.CompanyName())
                .RuleFor(r => r.Description, f => f.Lorem.Sentence())
                .RuleFor(r => r.Price, f => f.Random.Int(50, 500))
                .RuleFor(r => r.HotelId, hotelId);

            return roomFaker.Generate();
        }
        else
        {
            var roomFaker = new Faker<RoomRequest>()
                .RuleFor(r => r.Capacity, f => f.Random.Int(1, 10))
                .RuleFor(r => r.Name, f => f.Company.CompanyName())
                .RuleFor(r => r.Description, f => f.Lorem.Sentence())
                .RuleFor(r => r.Price, f => f.Random.Int(50, 500))
                .RuleFor(r => r.HotelId, hotelId)
                .RuleFor(r => r.Id, roomId);

            return roomFaker.Generate();
        }
    }

    public static Room GenerateRoom(string hotelId, string roomId)
    {
        var roomFaker = new Faker<Room>()
            .RuleFor(r => r.Capacity, f => f.Random.Int(1, 10))
            .RuleFor(r => r.Name, f => f.Lorem.Sentence())
            .RuleFor(r => r.Description, f => f.Lorem.Sentence())
            .RuleFor(r => r.Price, f => f.Random.Int(50, 500))
            .RuleFor(r => r.HotelId, hotelId)
            .RuleFor(r => r.Id, roomId);

        return roomFaker.Generate();
    }

    public static Hotel GenerateHotel(string id)
    {
        var hotelFaker = new Faker<Hotel>()
            .RuleFor(h => h.Name, f => f.Company.CompanyName())
            .RuleFor(h => h.Id, id)
            .RuleFor(h => h.Description, f => f.Lorem.Sentence())
            .RuleFor(h => h.Address, f => f.Address.StreetAddress())
            .RuleFor(h => h.City, f => f.Address.City())
            .RuleFor(h => h.PhoneNumber, f => f.Phone.PhoneNumber())
            .RuleFor(h => h.Stars, f => f.Random.Int(1, 5));

        return hotelFaker.Generate();
    }

    public static HotelRequest GenerateHotelRequest(string hotelId)
    {
        var hotelFaker = new Faker<HotelRequest>()
            .RuleFor(h => h.Name, f => f.Company.CompanyName())
            .RuleFor(h => h.Id, hotelId)
            .RuleFor(h => h.Description, f => f.Lorem.Sentence())
            .RuleFor(h => h.Address, f => f.Address.StreetAddress())
            .RuleFor(h => h.City, f => f.Address.City())
            .RuleFor(h => h.PhoneNumber, f => f.Phone.PhoneNumber())
            .RuleFor(h => h.Stars, f => f.Random.Int(1, 5));

        return hotelFaker.Generate();
    }

    public static HotelResponse GenerateHotelResponse(string hotelId)
    {
        var hotelFaker = new Faker<HotelResponse>()
            .RuleFor(h => h.Name, f => f.Company.CompanyName())
            .RuleFor(h => h.Id, hotelId)
            .RuleFor(h => h.Description, f => f.Lorem.Sentence())
            .RuleFor(h => h.Address, f => f.Address.StreetAddress())
            .RuleFor(h => h.City, f => f.Address.City())
            .RuleFor(h => h.PhoneNumber, f => f.Phone.PhoneNumber());

        return hotelFaker.Generate();
    }


    public static ReservationDto GenerateReservationDto(string hotelId, string roomId, DateTime checkInTime,
        DateTime checkOutTime)
    {
        var reservationDto = new Faker<ReservationDto>()
            .RuleFor(r => r.CheckInDate, f => f.Date.Future())
            .RuleFor(r => r.CheckOutDate, f => f.Date.Future())
            .RuleFor(r => r.HotelId, f => hotelId)
            .RuleFor(r => r.RoomId, f => roomId)
            .RuleFor(r => r.UserEmail, f => f.Internet.Email())
            .Generate();

        return reservationDto;
    }


    public static ReservationRequest GenerateReservationRequest(string hotelId, string roomId, DateTime checkInTime,
        DateTime checkOutTime, string userEmail)
    {
        var reservationRequest = new Faker<ReservationRequest>()
            .RuleFor(r => r.CheckInDate, f => checkInTime)
            .RuleFor(r => r.CheckOutDate, f => checkOutTime)
            .RuleFor(r => r.HotelId, f => hotelId)
            .RuleFor(r => r.RoomId, f => roomId)
            .RuleFor(r => r.UserEmail, f => userEmail)
            .Generate();

        return reservationRequest;
    }

    public static Reservation GenerateReservation(string hotelId, string roomId, DateTime checkInTime,
        DateTime checkOutTime)
    {
        var reservation = new Faker<Reservation>()
            .RuleFor(r => r.CheckInDate, checkInTime)
            .RuleFor(r => r.CheckOutDate, checkOutTime)
            .RuleFor(r => r.HotelId, f => hotelId)
            .RuleFor(r => r.RoomId, roomId)
            .RuleFor(r => r.UserEmail, f => f.Internet.Email())
            .Generate();

        return reservation;
    }

    public static Comment GenerateComment(string userEmail, string hotelId, string commentId)
    {
        var commentFaker = new Faker<Comment>()
            .RuleFor(c => c.Description, f => f.Lorem.Sentence())
            .RuleFor(c => c.UserEmail, f => userEmail)
            .RuleFor(c => c.Id, commentId)
            .RuleFor(c => c.HotelId, hotelId);

        return commentFaker.Generate();
    }

    public static CommentDto GenerateCommentDto(string userEmail, string hotelId, string commentId)
    {
        var commentFaker = new Faker<CommentDto>()
            .RuleFor(c => c.Id, f => commentId)
            .RuleFor(c => c.Description, f => f.Lorem.Sentence())
            .RuleFor(c => c.UserEmail, f => userEmail)
            .RuleFor(c => c.HotelId, f => hotelId);

        return commentFaker.Generate();
    }
}