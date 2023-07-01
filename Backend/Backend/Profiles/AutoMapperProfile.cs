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

        private List<string> GetHotelImages(string hotelId)
        {
            var baseUri = "http://localhost:8088";

            var filePath = baseUri + "/res/hotels" + hotelId;

            if (System.IO.File.Exists(filePath))
            {
                var imageFiles = Directory.GetFiles(filePath, "*.jpg", SearchOption.TopDirectoryOnly).ToList();
                return imageFiles;
            }
            return new List<string> { $"{baseUri}/images/image_missing.png" };
        }
    }
}