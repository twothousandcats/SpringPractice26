using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Tests.Entities;

public class ReservationTests
{
    private const string Currency = "EUR";

    private const string GuestName = "Ivan Ivanov";

    private const string GuestNumber = "+79991234567";

    private static readonly DateOnly ArrivalDate = new DateOnly( 2026, 6, 1 );

    private static readonly DateOnly DepartureDate = new DateOnly( 2026, 6, 4 );

    private static readonly TimeOnly ArrivalTime = new TimeOnly( 14, 0 );

    private static readonly TimeOnly DepartureTime = new TimeOnly( 12, 0 );

    private static Reservation CreateValidReservation(
        Money? money = null,
        DateRange? stay = null
    )
    {
        return new Reservation(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            stay ?? new DateRange( ArrivalDate, DepartureDate ),
            ArrivalTime,
            DepartureTime,
            GuestName,
            GuestNumber,
            money ?? new Money( 100m, Currency )
        );
    }

    [Fact]
    public void Constructor_ValidArguments_CreatesReservation()
    {
        Guid id = Guid.NewGuid();
        Guid propertyId = Guid.NewGuid();
        Guid roomTypeId = Guid.NewGuid();
        DateRange stay = new DateRange( ArrivalDate, DepartureDate );
        Money dailyPrice = new Money( 150m, Currency );

        Reservation reservation = new Reservation(
            id,
            propertyId,
            roomTypeId,
            stay,
            ArrivalTime,
            DepartureTime,
            GuestName,
            GuestNumber,
            dailyPrice
        );

        Assert.Equal( id, reservation.Id );
        Assert.Equal( propertyId, reservation.PropertyId );
        Assert.Equal( roomTypeId, reservation.RoomTypeId );
        Assert.Equal( stay, reservation.Period );
        Assert.Equal( ArrivalTime, reservation.ArrivalTime );
        Assert.Equal( DepartureTime, reservation.DepartureTime );
        Assert.Equal( GuestName, reservation.GuestName );
        Assert.Equal( GuestNumber, reservation.GuestPhoneNumber );
        Assert.Equal( ReservationStatus.Active, reservation.Status );
    }

    [Fact]
    public void Constructor_ComputesTotalAsDailyPriceTimesNights()
    {
        DateRange stay = new DateRange( ArrivalDate, DepartureDate );
        Money dailyPrice = new Money( 150m, Currency );

        Reservation reservation = CreateValidReservation( dailyPrice, stay );

        Assert.Equal( new Money( 450m, Currency ), reservation.Total );
    }

    [Fact]
    public void Constructor_EmptyId_Throws()
    {
        Assert.Throws<ArgumentException>( () => new Reservation(
                Guid.Empty,
                Guid.NewGuid(),
                Guid.NewGuid(),
                new DateRange( ArrivalDate, DepartureDate ),
                ArrivalTime,
                DepartureTime,
                GuestName,
                GuestNumber,
                new Money( 100m, Currency )
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
                new DateRange( ArrivalDate, DepartureDate ),
                ArrivalTime,
                DepartureTime,
                GuestName,
                GuestNumber,
                new Money( 100m, Currency )
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
                new DateRange( ArrivalDate, DepartureDate ),
                ArrivalTime,
                DepartureTime,
                GuestName,
                GuestNumber,
                new Money( 100m, Currency )
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
                ArrivalTime,
                DepartureTime,
                GuestName,
                GuestNumber,
                new Money( 100m, Currency )
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
                new DateRange( ArrivalDate, DepartureDate ),
                ArrivalTime,
                DepartureTime,
                GuestName,
                GuestNumber,
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
                new DateRange( ArrivalDate, DepartureDate ),
                ArrivalTime,
                DepartureTime,
                name!,
                GuestNumber,
                new Money( 100m, Currency )
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
                new DateRange( ArrivalDate, DepartureDate ),
                ArrivalTime,
                DepartureTime,
                GuestName,
                phone!,
                new Money( 100m, Currency )
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