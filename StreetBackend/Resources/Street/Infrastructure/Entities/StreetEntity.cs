using System;
namespace StreetBackend.Resources.Street.Infrastructure.Entities
{
	public class StreetEntity
	{
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public required string GeometryWkt { get; set; } // WKT representation for EF/PostGIS
        public required byte[] RowVersion { get; set; } // For concurrency control

    }
}

