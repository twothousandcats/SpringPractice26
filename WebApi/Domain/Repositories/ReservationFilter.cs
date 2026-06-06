using Domain.ValueObjects;

namespace Domain.Repositories;

public record ReservationFilter(
    Guid? PropertyId = null,
    Guid? RoomTypeId = null,
    string? GuestName = null,
    DateRange? Period = null
);