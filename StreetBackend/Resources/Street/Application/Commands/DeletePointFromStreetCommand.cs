using System;
using StreetBackend.Common.Interfaces;
using StreetBackend.Resources.Street.API.DTOs;
using StreetBackend.Resources.Street.Domain;

namespace StreetBackend.Resources.Street.Application.Commands
{
	public class DeletePointFromStreetCommand: ICommand
	{
        public Guid StreetId { get; set; }
        public required CoordinateDto PointToRemove { get; set; }
    }
}

