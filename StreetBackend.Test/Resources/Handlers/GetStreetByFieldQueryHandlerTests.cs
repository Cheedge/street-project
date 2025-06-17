using System;
using StreetBackend.Resources.Street.API.DTOs;
using StreetBackend.Resources.Street.Infrastructure.Repositories;
using System.Threading.Tasks;
using Moq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using StreetBackend.Resources.Street.Application.Queries;
using StreetBackend.Resources.Street.Domain;
using StreetBackend.Resources.Street.Application.QueryHandlers;

namespace StreetBackend.Test.Resources.Handlers
{
    public class GetStreetByFieldQueryHandlerTests
    {
        [Fact]
        public async Task HandleAsync_ShouldReturnStreetDto()
        {
            var street = StreetDomain.CreateStreet("A", 10, new[] { new Coordinate(0, 0), new Coordinate(0.5, 0.5) });

            var repo = new Mock<IStreetRepository>();
            var logger = new Mock<ILogger<GetStreetByFieldQueryHandler>>();
            var mapper = new Mock<IMapper>();

            repo.Setup(r => r.GetByIdAsync(street.Id)).ReturnsAsync(street);
            //mapper.Setup(m => m.Map<StreetDto>(street)).Returns(new StreetDto { Name = "A" });
            mapper.Setup(m => m.Map<StreetDto>(street)).Returns(new StreetDto
            {
                Name = "A",
                Coordinates = new List<CoordinateDto> { new CoordinateDto { X = 0, Y = 0 } }
            });


            var handler = new GetStreetByFieldQueryHandler(repo.Object, logger.Object, mapper.Object);

            var query = new GetStreetByFieldQuery { Id = street.Id };

            var result = await handler.HandleAsync(query);

            Assert.NotNull(result);
            Assert.Equal("A", result.Name);
        }
    }
}

