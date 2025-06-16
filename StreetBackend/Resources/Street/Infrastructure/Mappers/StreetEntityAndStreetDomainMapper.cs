using System;
using AutoMapper;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using StreetBackend.Resources.Street.Domain;
using StreetBackend.Resources.Street.Infrastructure.Entities;
using Coordinate = StreetBackend.Resources.Street.Domain.Coordinate;

namespace StreetBackend.Resources.Street.Infrastructure.Mappers
{
	public class StreetEntityAndStreetDomainMapper: Profile
	{
        public StreetEntityAndStreetDomainMapper()
        {
            CreateMap<StreetDomain, StreetEntity>()
                .ForMember(dest => dest.GeometryWkt, opt => opt.MapFrom(src =>
                    new WKTWriter().Write(
                        new LineString(src.Geometry.Geometry.Coordinates
                            .Select(p => new NetTopologySuite.Geometries.Coordinate(p.Y, p.X))
                            .ToArray())
                    )))
                //.ForMember(dest => dest.RowVersion, opt => opt.Ignore());
                .ForMember(dest => dest.RowVersion, opt => opt.MapFrom(src => src.RowVersion));

            CreateMap<StreetEntity, StreetDomain>()
                .ConstructUsing(src => MapStreetEntityToDomain(src));

        }

        private static StreetDomain MapStreetEntityToDomain(StreetEntity src)
        {
            var line = (LineString)new WKTReader().Read(src.GeometryWkt);
            var coords = line.Coordinates.Select(c => new Coordinate(c.Y, c.X));
            var street = StreetDomain.CreateStreet(src.Name, src.Capacity, coords);
            street.SetRowVersion(src.RowVersion);
            return street;
        }
    }
}

