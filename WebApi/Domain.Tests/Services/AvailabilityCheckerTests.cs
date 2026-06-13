using Domain.Entities;
using Domain.Repositories;
using Domain.Services;
using Domain.ValueObjects;
using Moq;

namespace Domain.Tests.Services;

public class AvailabilityCheckerTests
{
    [Fact]
    public void CanBook_NoOverlap_ReturnsTrue()
    {
        // Arrange
        Mock<IReservationRepository> reservationRepositoryMock = new Mock<IReservationRepository>();
        DateRange period = new DateRange(
            new DateOnly( 2020, 01, 01 ),
            new DateOnly( 2020, 01, 02 )
        );

        RoomType roomType = new RoomType(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Test name",
            new Money( 100m, "Test currency" ),
            1,
            2,
            2,
            new[] { "Breakfast" },
            new[] { "Wi-Fi", "TV" }
        );

        reservationRepositoryMock
            .Setup( repo => repo.GetOverlapping( roomType.Id, period ) )
            .Returns( Array.Empty<Reservation>() );

        AvailabilityChecker sut = new AvailabilityChecker( reservationRepositoryMock.Object );

        // Act
        bool result = sut.CanBook( roomType, period );

        // Assert
        Assert.True( result );
    }

    [Fact]
    public void CanBook_ActiveOverlapBelowCapacity_ReturnsTrue()
    {
        // Arrange
        Mock<IReservationRepository> reservationRepositoryMock = new Mock<IReservationRepository>();
        DateRange range = new DateRange(
            new DateOnly( 2020, 01, 01 ),
            new DateOnly( 2020, 01, 02 )
        );

        Reservation reservation = new Reservation(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            range,
            new TimeOnly( 10, 20, 30 ),
            new TimeOnly( 10, 20, 30 ),
            "Test guest",
            "88005553535",
            new Money( 100m, "Test currency" )
        );

        RoomType roomType = new RoomType(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Test name",
            new Money( 100m, "Test currency" ),
            1,
            2,
            2,
            new[] { "Breakfast" },
            new[] { "Wi-Fi", "TV" }
        );

        DateRange period = new DateRange(
            new DateOnly( 2020, 01, 01 ),
            new DateOnly( 2020, 01, 02 )
        );

        reservationRepositoryMock
            .Setup( r => r.GetOverlapping( roomType.Id, period ) )
            .Returns( new[] { reservation } );

        AvailabilityChecker sut = new AvailabilityChecker( reservationRepositoryMock.Object );

        // Act
        bool result = sut.CanBook( roomType, period );

        // Assert
        Assert.True( result );
    }

    [Fact]
    public void CanBook_ActiveOverlapAtCapacity_ReturnsFalse()
    {
        // Arrange
        Mock<IReservationRepository> reservationRepositoryMock = new Mock<IReservationRepository>();
        RoomType roomType = new RoomType(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Test name",
            new Money( 100m, "Test currency" ),
            1,
            2,
            2,
            new[] { "Breakfast" },
            new[] { "Wi-Fi", "TV" }
        );

        DateRange period = new DateRange(
            new DateOnly( 2020, 01, 01 ),
            new DateOnly( 2020, 01, 02 )
        );

        Reservation firstReservation = new Reservation(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            period,
            new TimeOnly( 10, 20, 30 ),
            new TimeOnly( 10, 20, 30 ),
            "Test guest",
            "88005553535",
            new Money( 100m, "Test currency" )
        );

        Reservation secondReservation = new Reservation(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            period,
            new TimeOnly( 10, 20, 30 ),
            new TimeOnly( 10, 20, 30 ),
            "Test guest",
            "88005553535",
            new Money( 100m, "Test currency" )
        );

        reservationRepositoryMock
            .Setup( r => r.GetOverlapping( roomType.Id, period ) )
            .Returns( new[] { firstReservation, secondReservation } );

        AvailabilityChecker sut = new AvailabilityChecker( reservationRepositoryMock.Object );

        // Act
        bool result = sut.CanBook( roomType, period );

        // Assert
        Assert.False( result );
    }

    [Fact]
    public void CanBook_CancelledReservationsDoNotCount()
    {
        // Arrange
        Mock<IReservationRepository> reservationRepositoryMock = new Mock<IReservationRepository>();
        RoomType roomType = new RoomType(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Test name",
            new Money( 100m, "Test currency" ),
            1,
            2,
            2,
            new[] { "Breakfast" },
            new[] { "Wi-Fi", "TV" }
        );

        DateRange period = new DateRange(
            new DateOnly( 2020, 01, 01 ),
            new DateOnly( 2020, 01, 02 )
        );

        Reservation firstReservation = new Reservation(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            period,
            new TimeOnly( 10, 20, 30 ),
            new TimeOnly( 10, 20, 30 ),
            "Test guest",
            "88005553535",
            new Money( 100m, "Test currency" )
        );

        Reservation secondReservation = new Reservation(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            period,
            new TimeOnly( 10, 20, 30 ),
            new TimeOnly( 10, 20, 30 ),
            "Test guest",
            "88005553535",
            new Money( 100m, "Test currency" )
        );

        reservationRepositoryMock
            .Setup( r => r.GetOverlapping( roomType.Id, period ) )
            .Returns( new[] { firstReservation, secondReservation } );

        firstReservation.Cancel();
        secondReservation.Cancel();
        reservationRepositoryMock
            .Setup( r => r.GetOverlapping( roomType.Id, period ) )
            .Returns( new[] { firstReservation, secondReservation } );

        AvailabilityChecker sut = new AvailabilityChecker( reservationRepositoryMock.Object );

        // Act
        bool result = sut.CanBook( roomType, period );

        // Assert
        Assert.True( result );
    }
}