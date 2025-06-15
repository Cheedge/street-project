using System;
using System.ComponentModel.DataAnnotations;
using NetTopologySuite.Geometries;

namespace StreetBackend.Resources.Street.Domain
{
	public class StreetDomain
	{
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public int Capacity { get; private set; }
        public GeometryVo Geometry { get; private set; }

        [Timestamp]
        public byte[] RowVersion { get; private set; }

        private StreetDomain(
            Guid id,
            string name,
            int capacity,
            GeometryVo geometry,
            byte[] rowVersion)
        {
            Id = id;
            Name = name;
            Capacity = capacity;
            Geometry = geometry;
            RowVersion = rowVersion;
        }

        /// <summary>
        /// Create the new Street
        /// Notice the RowVersion is not set yet, use it as placeholder,
        /// wait for DB to assign a RowVersion
        /// </summary>
        /// <param name="name"></param>
        /// <param name="capacity"></param>
        /// <param name="points"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static StreetDomain CreateStreet(string name, int capacity, IEnumerable<Coordinate> points)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Street name is required");

            if (capacity <= 0)
                throw new ArgumentException("Capacity must be positive");

            var geometry = new GeometryVo(points);

            return new StreetDomain(Guid.NewGuid(), name, capacity, geometry, Array.Empty<byte>());
        }

        public void AddPointToEnd(Coordinate point)
        {
            Geometry = Geometry.AddPointToEnd(point);
        }

        public void AddPointToStart(Coordinate point)
        {
            Geometry = Geometry.AddPointToStart(point);
        }

        /// <summary>
        /// Determines whether the point is closer to the start or end of the geometry.
        /// </summary>
        /// <param name="point"></param>
        /// <returns>Returns true if closer to start, false if closer to end.</returns>
        public bool IsCloserToStart(Coordinate point)
        {
            var start = Geometry.GetStartPoint();
            var end = Geometry.GetEndPoint();

            var distToStart = point.DistanceTo(start);
            var distToEnd = point.DistanceTo(end);

            return distToStart < distToEnd;
        }

        public void SetRowVersion(byte[] version)
        {
            RowVersion = version;
        }
    }
}

