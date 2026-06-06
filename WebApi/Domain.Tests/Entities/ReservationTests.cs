using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Tests.Entities;

public class ReservationTests
{
    private const string DefaultCurrency = "EUR";

    private const string DefaultGuestName = "Ivan Ivanov";

    private const string DefaultGuestNumber = "+79991234567";

    private static readonly DateOnly DefaultArrival = new DateOnly( 2026, 6, 1 );

    private static readonly DateOnly DefaultDeparture = new DateOnly( 2026, 6, 4 );

    private static readonly TimeOnly DefaultArrivalTime = new TimeOnly( 14, 0 );

    private static readonly TimeOnly DefaultDepartureTime = new TimeOnly( 12, 0 );

    private static Reservation CreateValidReservation(
        Money? money = null,
        DateRange? stay = null
    )
    {
        return new Reservation(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            stay ?? new DateRange( DefaultArrival, DefaultDeparture ),
            DefaultArrivalTime,
            DefaultDepartureTime,
            DefaultGuestName,
            DefaultGuestNumber,
            money ?? new Money( 100m, DefaultCurrency )
        );
    }

    [Fact]
    public void Constructor_ValidArguments_CreatesReservation()
    {
        Guid id = Guid.NewGuid();
        Guid propertyId = Guid.NewGuid();
        Guid roomTypeId = Guid.NewGuid();
        DateRange stay = new DateRange( DefaultArrival, DefaultDeparture );
        Money dailyPrice = new Money( 150m, DefaultCurrency );

        Reservation reservation = new Reservation(
            id,
            propertyId,
            roomTypeId,
            stay,
            DefaultArrivalTime,
            DefaultDepartureTime,
            DefaultGuestName,
            DefaultGuestNumber,
            dailyPrice
        );

        Assert.Equal( id, reservation.Id );
        Assert.Equal( propertyId, reservation.PropertyId );
        Assert.Equal( roomTypeId, reservation.RoomTypeId );
        Assert.Equal( stay, reservation.Stay );
        Assert.Equal( DefaultArrivalTime, reservation.ArrivalTime );
        Assert.Equal( DefaultDepartureTime, reservation.DepartureTime );
        Assert.Equal( DefaultGuestName, reservation.GuestName );
        Assert.Equal( DefaultGuestNumber, reservation.GuestPhoneNumber );
        Assert.Equal( ReservationStatus.Active, reservation.Status );
    }

    [Fact]
    public void Constructor_ComputesTotalAsDailyPriceTimesNights()
    {
        DateRange stay = new DateRange( DefaultArrival, DefaultDeparture );
        Money dailyPrice = new Money( 150m, DefaultCurrency );

        Reservation reservation = CreateValidReservation( dailyPrice, stay );

        Assert.Equal( new Money( 450m, DefaultCurrency ), reservation.Total );
    }

    [Fact]
    public void Constructor_EmptyId_Throws()
    {
        Assert.Throws<ArgumentException>( () => new Reservation(
                Guid.Empty,
                Guid.NewGuid(),
                Guid.NewGuid(),
                new DateRange( DefaultArrival, DefaultDeparture ),
                DefaultArrivalTime,
                DefaultDepartureTime,
                DefaultGuestName,
                DefaultGuestNumber,
                new Money( 100m, DefaultCurrency )
            )
        );
    }

    [Fact]
    public void Constructor_EmptyPropertyId_Throws()
    {
        Assert.Throws<ArgumentException>( () => new Reservation(
                Guid.NewGuid(),
                Guid.Empty,
                Guid.NewGuid(),
                new DateRange( DefaultArrival, DefaultDeparture ),
                DefaultArrivalTime,
                DefaultDepartureTime,
                DefaultGuestName,
                DefaultGuestNumber,
                new Money( 100m, DefaultCurrency )
            )
        );
    }

    [Fact]
    public void Constructor_EmptyRoomTypeId_Throws()
    {
        Assert.Throws<ArgumentException>( () => new Reservation(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.Empty,
                new DateRange( DefaultArrival, DefaultDeparture ),
                DefaultArrivalTime,
                DefaultDepartureTime,
                DefaultGuestName,
                DefaultGuestNumber,
                new Money( 100m, DefaultCurrency )
            )
        );
    }

    [Fact]
    public void Constructor_NullStay_Throws()
    {
        Assert.Throws<ArgumentNullException>( () => new Reservation(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                null!,
                DefaultArrivalTime,
                DefaultDepartureTime,
                DefaultGuestName,
                DefaultGuestNumber,
                new Money( 100m, DefaultCurrency )
            )
        );
    }

    [Fact]
    public void Constructor_NullDailyPrice_Throws()
    {
        Assert.Throws<ArgumentNullException>( () => new Reservation(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                new DateRange( DefaultArrival, DefaultDeparture ),
                DefaultArrivalTime,
                DefaultDepartureTime,
                DefaultGuestName,
                DefaultGuestNumber,
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
        Assert.Throws<ArgumentException>( () => new Reservation(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                new DateRange( DefaultArrival, DefaultDeparture ),
                DefaultArrivalTime,
                DefaultDepartureTime,
                name!,
                DefaultGuestNumber,
                new Money( 100m, DefaultCurrency )
            )
        );
    }

    [Theory]
    [InlineData( "" )]
    [InlineData( "   " )]
    [InlineData( null )]
    public void Constructor_InvalidGuestPhone_Throws( string? phone )
    {
        Assert.Throws<ArgumentException>( () => new Reservation(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                new DateRange( DefaultArrival, DefaultDeparture ),
                DefaultArrivalTime,
                DefaultDepartureTime,
                DefaultGuestName,
                phone!,
                new Money( 100m, DefaultCurrency )
            )
        );
    }

    [Fact]
    public void Cancel_ActiveReservation_SwitchesStatusToCancelled()
    {
        Reservation reservation = CreateValidReservation();

        reservation.Cancel();

        Assert.Equal( ReservationStatus.Cancelled, reservation.Status );
    }

    [Fact]
    public void Cancel_AlreadyCancelled_Idempotent()
    {
        Reservation reservation = CreateValidReservation();
        reservation.Cancel();
        ReservationStatus status = reservation.Status;
        reservation.Cancel();

        Assert.Equal( ReservationStatus.Cancelled, status );
    }
}