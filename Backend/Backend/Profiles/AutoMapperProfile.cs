using AutoMapper;
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
            var directoryPath = "./images";
            var files = Directory.GetFiles(directoryPath, hotelId + "_*.jpg").ToList();
            var allImageFilePaths = files.Select(file => Path.Combine(baseUri, file)).ToList();

            return allImageFilePaths;
        }
    }
}