using Domain.ValueObjects;

namespace Domain.Services;

public sealed record ReservationInput(
    Guid PropertyId,
    Guid RoomTypeId,
    DateRange Period,
    TimeOnly ArrivalDate,
    TimeOnly DepartureDate,
    int GuestCount,
    string GuestName,
    string GuestPhoneNumber
);