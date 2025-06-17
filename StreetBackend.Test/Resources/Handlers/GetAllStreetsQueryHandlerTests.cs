using System;
using StreetBackend.Resources.Street.API.DTOs;
using StreetBackend.Resources.Street.Infrastructure.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using StreetBackend.Resources.Street.Application.QueryHandlers;
using StreetBackend.Resources.Street.Domain;
using StreetBackend.Resources.Street.Application.Queries;

namespace StreetBackend.Test.Resources.Handlers
{
    public class GetAllStreetsQueryHandlerTests
    {
        [Fact]
        public async Task HandleAsync_ShouldReturnMappedDtos()
        {
            var repo = new Mock<IStreetRepository>();
            var logger = new Mock<ILogger<GetAllStreetsQueryHandler>>();
            var mapper = new Mock<IMapper>();

            var domainList = new List<StreetDomain>
            {
                StreetDomain.CreateStreet("A", 10, new[] { new Coordinate(0, 0), new Coordinate(0.5, 0.5) })
            };

            mapper.Setup(m => m.Map<StreetDto>(It.IsAny<StreetDomain>()))
                  .Returns((StreetDomain src) => new StreetDto
                  {
                      // Map the name from the source domain object
                      Name = src.Name,
                      Coordinates = src.Geometry.Geometry.Coordinates.Select(c => new CoordinateDto { X = c.X, Y = c.Y }).ToList()
                  });

            repo.Setup(r => r.GetAllAsync()).ReturnsAsync(domainList);

            var handler = new GetAllStreetsQueryHandler(repo.Object, logger.Object, mapper.Object);

            var result = await handler.HandleAsync(new GetAllStreetsQuery());

            Assert.Single(result);
            Assert.Equal("A", result.First().Name);
        }
    }
}