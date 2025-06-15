using System;
using StreetBackend.Common.Interfaces;

namespace StreetBackend.Resources.Street.Application.Commands
{
	public class DeleteStreetCommand: ICommand
	{
		public Guid StreetId { get; set; }
    }
}

