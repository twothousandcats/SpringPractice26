using Domain.ValueObjects;

namespace Domain.Services;

public sealed record ReservationInput(
    Guid PropertyId,
    Guid RoomTypeId,
    DateRange Period,
    TimeOnly ArrivalTime,
    TimeOnly DepartureTime,
    int GuestCount,
    string GuestName,
    string GuestPhoneNumber
);