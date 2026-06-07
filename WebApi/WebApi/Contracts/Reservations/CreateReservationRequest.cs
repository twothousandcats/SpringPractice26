using WebApi.Contracts.Common;

namespace WebApi.Contracts.Reservations;

public sealed record CreateReservationRequest(
    Guid PropertyId,
    Guid RoomTypeId,
    DateOnly ArrivalDate,
    DateOnly DepartureDate,
    TimeOnly ArrivalTime,
    TimeOnly DepartureTime,
    int GuestCount,
    string GuestName,
    string GuestPhoneNumber
);