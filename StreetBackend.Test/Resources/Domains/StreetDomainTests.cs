using StreetBackend.Resources.Street.Domain;
using Xunit;
using System;
using System.Linq;
using Moq;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace StreetBackend.Test.Resources.Domains;

public class StreetDomainTests
{
    [Fact]
    public void CreateStreet_WithValidData_ShouldSucceed()
    {
        var coords = new[] { new Coordinate(0, 0), new Coordinate(1, 1) };
        var street = StreetDomain.CreateStreet("TestStreet", 10, coords);

        Assert.Equal("TestStreet", street.Name);
        Assert.Equal(10, street.Capacity);
        Assert.NotNull(street.Geometry);
    }

    [Fact]
    public void CreateStreet_WithEmptyName_ShouldThrow()
    {
        var coords = new[] { new Coordinate(0, 0) };
        Assert.Throws<ArgumentException>(() => StreetDomain.CreateStreet("", 10, coords));
    }

    [Fact]
    public void AddPointToEnd_ShouldAppend()
    {
        // should be two points
        var coords = new[] { new Coordinate(0, 0), new Coordinate(0.5, 0.5) };
        var street = StreetDomain.CreateStreet("Test", 10, coords);

        street.AddPointToEnd(new Coordinate(1, 1));
        Assert.Equal(3, street.Geometry.Geometry.NumPoints);
    }

    [Fact]
    public void IsCloserToStart_ShouldReturnTrueOrFalse()
    {
        var coords = new[] { new Coordinate(0, 0), new Coordinate(10, 10) };
        var street = StreetDomain.CreateStreet("Test", 10, coords);

        Assert.True(street.IsCloserToStart(new Coordinate(1, 1)));
        Assert.False(street.IsCloserToStart(new Coordinate(9, 9)));
    }
}

