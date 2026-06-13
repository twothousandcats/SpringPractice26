using WebApi.Contracts.Common;

namespace WebApi.Contracts.RoomTypes;

public sealed record RoomTypeDto(
    Guid Id,
    Guid PropertyId,
    string Name,
    MoneyDto DailyPrice,
    int MinPersonCount,
    int MaxPersonCount,
    int TotalRooms,
    IReadOnlyCollection<string> Services,
    IReadOnlyCollection<string> Amenities
);