using System;
using NetTopologySuite.Geometries;

namespace StreetBackend.Resources.Street.Domain
{
	public class GeometryVo
	{
        public LineString Geometry { get; }

        public GeometryVo(IEnumerable<Coordinate> coordinates)
        {
            var ntsCoordinates = coordinates
                .Select(c => new NetTopologySuite.Geometries.Coordinate(c.Y, c.X))
                .ToArray();
            Geometry = new LineString(ntsCoordinates);
        }

        public GeometryVo AddPointToEnd(Coordinate point)
        {
            var coords = Geometry.Coordinates.ToList();
            coords.Add(new NetTopologySuite.Geometries.Coordinate(point.Y, point.X));
            return new GeometryVo(ToCoordinates(coords));
        }

        public GeometryVo AddPointToStart(Coordinate point)
        {
            var coords = Geometry.Coordinates.ToList();
            coords.Insert(0, new NetTopologySuite.Geometries.Coordinate(point.Y, point.X));
            return new GeometryVo(ToCoordinates(coords));
        }

        public Coordinate GetStartPoint()
        {
            var start = Geometry.StartPoint;
            return new Coordinate(start.X, start.Y);
        }

        public Coordinate GetEndPoint()
        {
            var end = Geometry.EndPoint;
            return new Coordinate(end.X, end.Y);
        }

        private IEnumerable<Coordinate> ToCoordinates(IEnumerable<NetTopologySuite.Geometries.Coordinate> nts)
        {
            return nts.Select(c => new Coordinate(c.X, c.Y)); // Longitude = X, Latitude = Y
        }

        public string AsWKT() => Geometry.AsText();
    }
}

