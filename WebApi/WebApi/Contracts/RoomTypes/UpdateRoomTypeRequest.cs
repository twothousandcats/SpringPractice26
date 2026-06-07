using WebApi.Contracts.Common;

namespace WebApi.Contracts.RoomTypes;

public record UpdateRoomTypeRequest(
    string Name,
    MoneyDto DailyPrice,
    int MinPersonCount,
    int MaxPersonCount,
    int TotalRooms,
    IReadOnlyCollection<string> Services,
    IReadOnlyCollection<string> Amenities
);