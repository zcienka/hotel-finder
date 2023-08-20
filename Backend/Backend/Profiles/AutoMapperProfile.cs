using AutoMapper;
using Backend.Dtos;
using Backend.Models;
using Backend.Requests;
using Backend.Responses;

namespace Backend.Profiles;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Hotel, HotelResponse>()
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => GetHotelImages(src.Id)));
        CreateMap<HotelResponse, Hotel>();
        CreateMap<HotelRequest, Hotel>();

        CreateMap<Comment, CommentDto>();
        CreateMap<CommentDto, Comment>();

        CreateMap<Room, RoomDto>().ForMember(dest => dest.Image, opt => opt.MapFrom(src => GetRoomImages(src.HotelId)));
        CreateMap<RoomDto, Room>();
        CreateMap<RoomRequest, Room>();


        CreateMap<Reservation, ReservationDto>()
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => GetRoomImages(src.HotelId)));
        CreateMap<ReservationDto, Reservation>();

        CreateMap<ReservationRequest, Reservation>();

        CreateMap<User, UserDto>();
        CreateMap<UserDto, User>();
    }

    private List<string> GetHotelImages(string hotelId)
    {
        var baseUri = "http://localhost:8088";
        var directoryPath = "./images";
        var files = Directory.GetFiles(directoryPath, hotelId + "_*.jpg").ToList();
        var imageFilePaths = files.Select(file => Path.Combine(baseUri, file)).ToList();

        return imageFilePaths;
    }

    private List<string> GetRoomImages(string hotelId)
    {
        var baseUri = "http://localhost:8088";
        var directoryPath = "./images";
        var files = Directory.GetFiles(directoryPath, hotelId + "_room" + "_*.jpg").ToList();
        var imageFilePaths = files.Select(file => Path.Combine(baseUri, file)).ToList();

        return imageFilePaths;
    }

  
}