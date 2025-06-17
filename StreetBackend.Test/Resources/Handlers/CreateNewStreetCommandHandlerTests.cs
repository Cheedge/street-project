using System;
using StreetBackend.Resources.Street.Application.Commands;
using StreetBackend.Resources.Street.Infrastructure.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using StreetBackend.Resources.Street.Application.CommandHandlers;
using StreetBackend.Resources.Street.Domain;
using StreetBackend.Resources.Street.API.DTOs;

namespace StreetBackend.Test.Resources.Handlers
{
    public class CreateNewStreetCommandHandlerTests
    {
        [Fact]
        public async Task HandleAsync_WithValidCommand_ShouldCallRepository()
        {
            var mockRepo = new Mock<IStreetRepository>();
            var mockLogger = new Mock<ILogger<CreateNewStreetCommandHandler>>();
            var handler = new CreateNewStreetCommandHandler(mockRepo.Object, mockLogger.Object);

            var cmd = new CreateNewStreetCommand
            {
                Name = "My Street No.1",
                Capacity = 5,
                Coordinates = new List<CoordinateDto>
                                {
                                    new CoordinateDto { X = 0, Y = 0 },
                                    new CoordinateDto { X = 1, Y = 1 }
                                }
            };

            await handler.HandleAsync(cmd);

            mockRepo.Verify(r => r.AddAsync(It.IsAny<StreetDomain>()), Times.Once);
        }
    }
}

