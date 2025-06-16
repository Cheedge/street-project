using System;
using StreetBackend.Common.Interfaces;
using StreetBackend.Resources.Street.API.DTOs;
using StreetBackend.Resources.Street.Domain;

namespace StreetBackend.Resources.Street.Application.Commands
{
	public class CreateNewStreetCommand: ICommand
	{
        //public Guid Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public required List<CoordinateDto> Coordinates { get; set; }
    }
}

