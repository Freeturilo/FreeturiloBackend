using AutoMapper;
using FreeturiloWebApi.DTO;
using FreeturiloWebApi.Models;

namespace FreeturiloWebApi.Helpers
{
    public class AutoMapperProfile : Profile
    {
        /// <summary>
        /// Add profiles to cast object
        /// </summary>
        public AutoMapperProfile()
        {
            CreateMap<Station, StationDTO>()
                .ForMember(dest => dest.Latitude, opts => opts.MapFrom(src => src.Lat))
                .ForMember(dest => dest.Longitude, opts => opts.MapFrom(src => src.Lon))
                .ForMember(dest => dest.Bikes, opts => opts.MapFrom(src => src.AvailableBikes))
                .ForMember(dest => dest.BikeRacks, opts => opts.MapFrom(src => src.AvailableRacks))
                .ForMember(dest => dest.Type, opts => opts.MapFrom(src => "Station"));

            CreateMap<StationDTO, Station>()
                .ForMember(dest => dest.Lat, opts => opts.MapFrom(src => src.Latitude))
                .ForMember(dest => dest.Lon, opts => opts.MapFrom(src => src.Longitude))
                .ForMember(dest => dest.AvailableRacks, opts => opts.MapFrom(src => src.BikeRacks))
                .ForMember(dest => dest.AvailableBikes, opts => opts.MapFrom(src => src.Bikes));

            CreateMap<Station, LocationDTO>()
                .ForMember(dest => dest.Latitude, opts => opts.MapFrom(src => src.Lat))
                .ForMember(dest => dest.Longitude, opts => opts.MapFrom(src => src.Lon))
                .ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Name))
                .ForMember(dest => dest.Type, opts => opts.MapFrom(src => "Station"));
        }
    }
}
