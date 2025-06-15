using System;
using AutoMapper;
using StreetBackend.Resources.Street.API.DTOs;
using StreetBackend.Resources.Street.Domain;
using Coordinate = StreetBackend.Resources.Street.Domain.Coordinate;

namespace StreetBackend.Resources.Street.Infrastructure.Mappers
{
	public class StreetDomainAndStreetDtoMapper: Profile
	{
        public StreetDomainAndStreetDtoMapper()
        {
            CreateMap<Coordinate, CoordinateDto>().ReverseMap();

            CreateMap<StreetDomain, StreetDto>()
                .ForMember(dest => dest.Coordinates, opt => opt.MapFrom(src => src.Geometry.Geometry.Coordinates))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.RowVersion, opt => opt.MapFrom(src => src.RowVersion))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Capacity, opt => opt.MapFrom(src => src.Capacity));

            CreateMap<StreetDto, StreetDomain>()
                .ConstructUsing(dto =>
                    StreetDomain.CreateStreet(dto.Name, dto.Capacity,
                        dto.Coordinates.Select(c => new Coordinate(c.Y, c.X))
                    ));

        }
    }
}

