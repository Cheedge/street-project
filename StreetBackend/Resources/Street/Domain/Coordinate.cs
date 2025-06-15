using System;
namespace StreetBackend.Resources.Street.Domain
{
	public class Coordinate
	{
        public double X { get; }
        public double Y { get; }

        public Coordinate(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double DistanceTo(Coordinate other)
        {
            var dx = this.X - other.X;
            var dy = this.Y - other.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
    }
}

