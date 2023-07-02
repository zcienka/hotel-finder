using AutoMapper;
using Azure.Core;
using Backend.Models;

namespace Backend.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Hotel, HotelResponse>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => GetHotelImages(src.Id)));
            CreateMap<HotelResponse, Hotel>();

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

            var filePath = baseUri + "/images/" + hotelId + ".jpg";

            return new List<string> { filePath };
        }
    }
}