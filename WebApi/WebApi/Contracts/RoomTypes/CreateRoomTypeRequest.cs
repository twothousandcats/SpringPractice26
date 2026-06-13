using WebApi.Contracts.Common;

namespace WebApi.Contracts.RoomTypes;

public sealed record CreateRoomTypeRequest(
    string Name,
    MoneyDto DailyPrice,
    int MinPersonCount,
    int MaxPersonCount,
    int TotalRooms,
    IReadOnlyCollection<string> Services,
    IReadOnlyCollection<string> Amenities
);