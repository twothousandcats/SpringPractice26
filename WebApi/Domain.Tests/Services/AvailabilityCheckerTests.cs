using Domain.Entities;
using Domain.Repositories;
using Domain.Services;
using Domain.ValueObjects;
using Moq;

namespace Domain.Tests.Services;

public class AvailabilityCheckerTests
{
    private const string Currency = "EUR";

    private const int TotalRooms = 2;

    private static readonly DateRange Period = new DateRange(
        new DateOnly( 2026, 6, 1 ),
        new DateOnly( 2026, 6, 4 )
    );

    private readonly Mock<IReservationRepository> _reservationRepositoryMock = new Mock<IReservationRepository>();

    private readonly AvailabilityChecker _checker;

    private readonly RoomType _roomType;

    private static Reservation CreateValidReservation( RoomType roomType, DateRange period )
    {
        return new Reservation(
            Guid.NewGuid(),
            roomType.PropertyId,
            roomType.Id,
            period,
            new TimeOnly( 14, 0 ),
            new TimeOnly( 12, 0 ),
            "Test Guest",
            "+71234567890",
            roomType.DailyPrice
        );
    }

    public AvailabilityCheckerTests()
    {
        _checker = new AvailabilityChecker( _reservationRepositoryMock.Object );
        _roomType = new RoomType(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Standard",
            new Money( 100m, Currency ),
            1,
            2,
            TotalRooms,
            Array.Empty<string>(),
            Array.Empty<string>()
        );
    }

    [Fact]
    public void CanBook_NoOverlap_ReturnsTrue()
    {
        _reservationRepositoryMock
            .Setup( r => r.GetOverlapping( _roomType.Id, Period ) )
            .Returns( Array.Empty<Reservation>() );

        bool result = _checker.CanBook( _roomType, Period );

        Assert.True( result );
    }

    [Fact]
    public void CanBook_ActiveOverlapBelowCapacity_ReturnsTrue()
    {
        Reservation existing = CreateValidReservation( _roomType, Period );
        _reservationRepositoryMock
            .Setup( r => r.GetOverlapping( _roomType.Id, Period ) )
            .Returns( new[] { existing } );

        bool result = _checker.CanBook( _roomType, Period );

        Assert.True( result );
    }

    [Fact]
    public void CanBook_ActiveOverlapAtCapacity_ReturnsFalse()
    {
        Reservation first = CreateValidReservation( _roomType, Period );
        Reservation second = CreateValidReservation( _roomType, Period );
        _reservationRepositoryMock
            .Setup( r => r.GetOverlapping( _roomType.Id, Period ) )
            .Returns( new[] { first, second } );

        bool result = _checker.CanBook( _roomType, Period );

        Assert.False( result );
    }

    [Fact]
    public void CanBook_CancelledReservationsDoNotCount()
    {
        Reservation first = CreateValidReservation( _roomType, Period );
        Reservation second = CreateValidReservation( _roomType, Period );
        first.Cancel();
        second.Cancel();
        _reservationRepositoryMock
            .Setup( r => r.GetOverlapping( _roomType.Id, Period ) )
            .Returns( new[] { first, second } );

        bool result = _checker.CanBook( _roomType, Period );

        Assert.True( result );
    }
}