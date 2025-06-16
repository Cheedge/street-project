using System;
namespace StreetBackend.Resources.Street.API.DTOs
{
	public class StreetDto
	{
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public required List<CoordinateDto> Coordinates { get; set; }
        public byte[] RowVersion { get; set; }
    }
}

