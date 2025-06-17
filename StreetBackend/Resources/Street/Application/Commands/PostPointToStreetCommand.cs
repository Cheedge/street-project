using System;
using StreetBackend.Common.Interfaces;
using StreetBackend.Resources.Street.API.DTOs;

namespace StreetBackend.Resources.Street.Application.Commands
{
	public class PostPointToStreetCommand: ICommand
	{
        public Guid StreetId { get; set; }
        public required CoordinateDto NewPoint { get; set; }
        public bool IfAddPointByStoredProcedualFlag { get; set; }
    }
}

