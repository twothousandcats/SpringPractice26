using WebApi.Contracts.Common;

namespace WebApi.Contracts.Reservations;

public sealed record ReservationDto(
    Guid Id,
    Guid PropertyId,
    Guid RoomTypeId,
    DateOnly ArrivalDate,
    DateOnly DepartureDate,
    TimeOnly ArrivalTime,
    TimeOnly DepartureTime,
    string GuestName,
    string GuestPhone,
    MoneyDto TotalPrice,
    string Status
);