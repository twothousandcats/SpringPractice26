using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Tests.Entities;

public class ReservationTests
{
    [Fact]
    public void Constructor_ValidArguments_CreatesReservation()
    {
        // Arrange
        Guid id = Guid.NewGuid();
        Guid propertyId = Guid.NewGuid();
        Guid roomTypeId = Guid.NewGuid();
        Money dailyPrice = new Money( 150m, "Test currency" );
        TimeOnly arrivalTime = new TimeOnly( 10, 20, 30 );
        TimeOnly departureTime = new TimeOnly( 10, 20, 30 );
        DateRange range = new DateRange(
            new DateOnly( 2020, 01, 01 ),
            new DateOnly( 2020, 01, 03 )
        );

        // Act
        Reservation sut = new Reservation(
            id,
            propertyId,
            roomTypeId,
            range,
            arrivalTime,
            departureTime,
            "Test guest",
            "88005553535",
            dailyPrice
        );

        // Assert
        Assert.Equal( id, sut.Id );
        Assert.Equal( propertyId, sut.PropertyId );
        Assert.Equal( roomTypeId, sut.RoomTypeId );
        Assert.Equal( range, sut.Period );
        Assert.Equal( arrivalTime, sut.ArrivalTime );
        Assert.Equal( departureTime, sut.DepartureTime );
        Assert.Equal( "Test guest", sut.GuestName );
        Assert.Equal( "88005553535", sut.GuestPhoneNumber );
        Assert.Equal( ReservationStatus.Active, sut.Status );
    }

    [Fact]
    public void Constructor_ComputesTotalAsDailyPriceTimesNights()
    {
        // Arrange
        Money dailyPrice = new Money( 100m, "Test currency" );
        DateRange range = new DateRange(
            new DateOnly( 2020, 01, 01 ),
            new DateOnly( 2020, 01, 04 )
        );

        // Act
        Reservation sut = new Reservation(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            range,
            new TimeOnly( 10, 20, 30 ),
            new TimeOnly( 10, 20, 30 ),
            "Test guest",
            "88005553535",
            dailyPrice
        );

        // Assert
        Assert.Equal( new Money( 300m, "Test currency" ), sut.Total );
    }

    [Fact]
    public void Constructor_EmptyId_Throws()
    {
        // Arrange
        DateRange range = new DateRange(
            new DateOnly( 2020, 01, 01 ),
            new DateOnly( 2020, 01, 02 )
        );

        // Act, Assert
        Assert.Throws<ArgumentException>( () => new Reservation(
                Guid.Empty,
                Guid.NewGuid(),
                Guid.NewGuid(),
                range,
                new TimeOnly( 10, 20, 30 ),
                new TimeOnly( 10, 20, 30 ),
                "Test guest",
                "88005553535",
                new Money( 100m, "Test currency" )
            )
        );
    }

    [Fact]
    public void Constructor_EmptyPropertyId_Throws()
    {
        // Arrange
        DateRange range = new DateRange(
            new DateOnly( 2020, 01, 01 ),
            new DateOnly( 2020, 01, 02 )
        );

        // Act, Assert
        Assert.Throws<ArgumentException>( () => new Reservation(
                Guid.NewGuid(),
                Guid.Empty,
                Guid.NewGuid(),
                range,
                new TimeOnly( 10, 20, 30 ),
                new TimeOnly( 10, 20, 30 ),
                "Test guest",
                "88005553535",
                new Money( 100m, "Test currency" )
            )
        );
    }

    [Fact]
    public void Constructor_EmptyRoomTypeId_Throws()
    {
        // Arrange
        DateRange range = new DateRange(
            new DateOnly( 2020, 01, 01 ),
            new DateOnly( 2020, 01, 02 )
        );

        // Act, Assert
        Assert.Throws<ArgumentException>( () => new Reservation(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.Empty,
                range,
                new TimeOnly( 10, 20, 30 ),
                new TimeOnly( 10, 20, 30 ),
                "Test guest",
                "88005553535",
                new Money( 100m, "Test currency" )
            )
        );
    }

    [Fact]
    public void Constructor_NullStay_Throws()
    {
        // Arrange
        // Act
        // Assert
        Assert.Throws<ArgumentNullException>( () => new Reservation(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                null!,
                new TimeOnly( 10, 20, 30 ),
                new TimeOnly( 10, 20, 30 ),
                "Test guest",
                "88005553535",
                new Money( 100m, "Test currency" )
            )
        );
    }

    [Fact]
    public void Constructor_NullDailyPrice_Throws()
    {
        // Arrange
        DateRange range = new DateRange(
            new DateOnly( 2020, 01, 01 ),
            new DateOnly( 2020, 01, 02 )
        );

        // Act
        // Assert
        Assert.Throws<ArgumentNullException>( () => new Reservation(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                range,
                new TimeOnly( 10, 20, 30 ),
                new TimeOnly( 10, 20, 30 ),
                "Test guest",
                "88005553535",
                null!
            )
        );
    }

    [Theory]
    [InlineData( "" )]
    [InlineData( "   " )]
    [InlineData( null )]
    public void Constructor_InvalidGuestName_Throws( string? name )
    {
        // Arrange
        DateRange range = new DateRange(
            new DateOnly( 2020, 01, 01 ),
            new DateOnly( 2020, 01, 02 )
        );

        // Act
        // Assert
        Assert.Throws<ArgumentException>( () => new Reservation(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                range,
                new TimeOnly( 10, 20, 30 ),
                new TimeOnly( 10, 20, 30 ),
                name!,
                "88005553535",
                new Money( 100m, "Test currency" )
            )
        );
    }

    [Theory]
    [InlineData( "" )]
    [InlineData( "   " )]
    [InlineData( null )]
    public void Constructor_InvalidGuestPhone_Throws( string? phone )
    {
        // Arrange
        DateRange range = new DateRange(
            new DateOnly( 2020, 01, 01 ),
            new DateOnly( 2020, 01, 02 )
        );

        // Act
        // Assert
        Assert.Throws<ArgumentException>( () => new Reservation(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                range,
                new TimeOnly( 10, 20, 30 ),
                new TimeOnly( 10, 20, 30 ),
                "Test guest",
                phone!,
                new Money( 100m, "Test currency" )
            )
        );
    }

    [Fact]
    public void Cancel_ActiveReservation_SwitchesStatusToCancelled()
    {
        // Arrange
        DateRange range = new DateRange(
            new DateOnly( 2020, 01, 01 ),
            new DateOnly( 2020, 01, 02 )
        );

        Reservation sut = new Reservation(
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

        // Act
        sut.Cancel();

        // Assert
        Assert.Equal( ReservationStatus.Cancelled, sut.Status );
    }

    [Fact]
    public void Cancel_AlreadyCancelled_Idempotent()
    {
        // Arrange
        DateRange range = new DateRange(
            new DateOnly( 2020, 01, 01 ),
            new DateOnly( 2020, 01, 02 )
        );

        Reservation sut = new Reservation(
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

        // Act
        sut.Cancel();
        ReservationStatus status = sut.Status;
        sut.Cancel();

        // Assert
        Assert.Equal( ReservationStatus.Cancelled, status );
    }
}