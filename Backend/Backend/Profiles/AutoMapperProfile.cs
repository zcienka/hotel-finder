using AutoMapper;
using Azure.Core;
using Backend.Models;

namespace Backend.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Hotel, HotelDto>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => GetHotelImages(src.Id)));
            CreateMap<HotelDto, Hotel>();

            CreateMap<Comment, CommentDto>();
            CreateMap<CommentDto, Comment>();

            CreateMap<Room, RoomDto>();
            CreateMap<RoomDto, Room>();

            CreateMap<Reservation, ReservationDto>();
            CreateMap<ReservationDto, Reservation>();
        }

        private string GetHotelImages(int hotelId)
        {
            var baseUri = "http://localhost:8088/api/v1";

            var filePath = baseUri + "/res/hotels" + hotelId;

            var imageUrl = System.IO.File.Exists(filePath)
                ? $"{filePath}/image.jpg"
                : $"{baseUri}/res/image_missing.png";

            return imageUrl;
        }
    }
}