using System;
using StreetBackend.Resources.Street.Application.Commands;
using StreetBackend.Resources.Street.Infrastructure.Repositories;
using System.Threading.Tasks;
using Moq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using StreetBackend.Resources.Street.Application.CommandHandlers;

namespace StreetBackend.Test.Resources.Handlers
{
    public class DeleteStreetCommandHandlerTests
    {
        [Fact]
        public async Task HandleAsync_ShouldCallDeleteStreetAsync()
        {
            var mockRepo = new Mock<IStreetRepository>();
            var mockLogger = new Mock<ILogger<DeleteStreetCommandHandler>>();
            var handler = new DeleteStreetCommandHandler(mockRepo.Object, mockLogger.Object);

            var command = new DeleteStreetCommand { StreetId = Guid.NewGuid() };

            await handler.HandleAsync(command);

            mockRepo.Verify(r => r.DeleteStreetAsync(command.StreetId), Times.Once);
        }
    }
}

