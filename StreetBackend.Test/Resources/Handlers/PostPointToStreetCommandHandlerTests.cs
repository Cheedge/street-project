using System;
using StreetBackend.Resources.Street.Application.Commands;
using StreetBackend.Resources.Street.Infrastructure.Repositories;
using System.Threading.Tasks;
using Moq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using StreetBackend.Resources.Street.Domain;
using StreetBackend.Resources.Street.Application.CommandHandlers;
using StreetBackend.Resources.Street.API.DTOs;

namespace StreetBackend.Test.Resources.Handlers
{
    public class PostPointToStreetCommandHandlerTests
    {
        [Fact]
        public async Task HandleAsync_WithStoredProcedure_ShouldCallUpdateByStoredProcedure()
        {
            var street = StreetDomain.CreateStreet("Test", 10, new[] { new Coordinate(0, 0), new Coordinate(0.5, 0.5) });

            var repo = new Mock<IStreetRepository>();
            var logger = new Mock<ILogger<PostPointToStreetCommandHandler>>();
            var mapper = new Mock<IMapper>();
            repo.Setup(r => r.GetByIdAsync(street.Id)).ReturnsAsync(street);

            var handler = new PostPointToStreetCommandHandler(repo.Object, logger.Object, mapper.Object);

            var cmd = new PostPointToStreetCommand
            {
                StreetId = street.Id,
                NewPoint = new CoordinateDto { X = 1, Y = 1 },
                IfAddPointByStoredProcedualFlag = true
            };

            await handler.HandleAsync(cmd);

            repo.Verify(r => r.UpdateStreetByStoredProcedualAsync(street, It.IsAny<Coordinate>(), It.IsAny<bool>()), Times.Once);
        }

        [Fact]
        public async Task HandleAsync_WithAlgorithm_ShouldCallUpdateByAlgo()
        {
            var street = StreetDomain.CreateStreet("Test", 10, new[] { new Coordinate(0, 0), new Coordinate(0.5, 0.5) });

            var repo = new Mock<IStreetRepository>();
            var logger = new Mock<ILogger<PostPointToStreetCommandHandler>>();
            var mapper = new Mock<IMapper>();
            repo.Setup(r => r.GetByIdAsync(street.Id)).ReturnsAsync(street);

            var handler = new PostPointToStreetCommandHandler(repo.Object, logger.Object, mapper.Object);

            var cmd = new PostPointToStreetCommand
            {
                StreetId = street.Id,
                NewPoint = new CoordinateDto { X = 10, Y = 10 },
                IfAddPointByStoredProcedualFlag = false
            };

            await handler.HandleAsync(cmd);

            repo.Verify(r => r.UpdateStreetByAlgoAsync(street), Times.Once);
        }

        [Fact]
        public async Task HandleAsync_StreetNotFound_ShouldThrow()
        {
            var repo = new Mock<IStreetRepository>();
            var logger = new Mock<ILogger<PostPointToStreetCommandHandler>>();
            var mapper = new Mock<IMapper>();
            repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((StreetDomain)null);

            var handler = new PostPointToStreetCommandHandler(repo.Object, logger.Object, mapper.Object);

            var cmd = new PostPointToStreetCommand
            {
                StreetId = Guid.NewGuid(),
                NewPoint = new CoordinateDto { X=0, Y=0},
                IfAddPointByStoredProcedualFlag = false
            };

            await Assert.ThrowsAsync<ArgumentNullException>(() => handler.HandleAsync(cmd));
        }
    }
}

