using Domain.ValueObjects;

namespace Domain.Entities;

public class Reservation
{
    public Guid Id { get; }

    public Guid PropertyId { get; }

    public Guid RoomTypeId { get; }

    public DateRange Period { get; }

    public TimeOnly ArrivalTime { get; }

    public TimeOnly DepartureTime { get; }

    public string GuestName { get; }

    public string GuestPhoneNumber { get; }

    public Money Total { get; }

    public ReservationStatus Status { get; private set; }

    public Reservation(
        Guid id,
        Guid propertyId,
        Guid roomTypeId,
        DateRange period,
        TimeOnly arrivalTime,
        TimeOnly departureTime,
        string guestName,
        string guestPhoneNumber,
        Money dailyPrice
    )
    {
        if ( id == Guid.Empty )
        {
            throw new ArgumentException( "Id is required", nameof( id ) );
        }

        if ( propertyId == Guid.Empty )
        {
            throw new ArgumentException( "PropertyId is required", nameof( propertyId ) );
        }

        if ( roomTypeId == Guid.Empty )
        {
            throw new ArgumentException( "RoomTypeId is required", nameof( roomTypeId ) );
        }

        Validate( period, dailyPrice, guestName, guestPhoneNumber );

        Id = id;
        PropertyId = propertyId;
        RoomTypeId = roomTypeId;
        Period = period;
        ArrivalTime = arrivalTime;
        DepartureTime = departureTime;
        GuestName = guestName;
        GuestPhoneNumber = guestPhoneNumber;
        Total = dailyPrice.Multiply( period.Nights );
        Status = ReservationStatus.Active;
    }

    public void Cancel()
    {
        if ( Status == ReservationStatus.Cancelled )
        {
            return;
        }

        Status = ReservationStatus.Cancelled;
    }

    private void Validate(
        DateRange period,
        Money dailyPrice,
        string guestName,
        string guestPhoneNumber
    )
    {
        ArgumentNullException.ThrowIfNull( period );
        ArgumentNullException.ThrowIfNull( dailyPrice );

        if ( string.IsNullOrWhiteSpace( guestName ) )
        {
            throw new ArgumentException( "Guest name is required", nameof( guestName ) );
        }

        if ( string.IsNullOrWhiteSpace( guestPhoneNumber ) )
        {
            throw new ArgumentException( "Guest phone number is required", nameof( guestPhoneNumber ) );
        }
    }
}