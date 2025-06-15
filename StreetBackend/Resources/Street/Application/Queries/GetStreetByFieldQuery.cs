using System;
using StreetBackend.Common.Interfaces;
using StreetBackend.Resources.Street.API.DTOs;

namespace StreetBackend.Resources.Street.Application.Queries
{
	public class GetStreetByFieldQuery: IQuery<StreetDto>
    {
        public Guid Id { get; set; }
    }
}

