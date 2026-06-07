using Domain.Entities;
using WebApi.Contracts.Common;

namespace WebApi.Contracts.RoomTypes;

public static class RoomTypeMappings
{
    public static RoomTypeDto ToDto( this RoomType roomType )
    {
        return new RoomTypeDto(
            roomType.Id,
            roomType.PropertyId,
            roomType.Name,
            roomType.DailyPrice.ToDto(),
            roomType.MinPersonCount,
            roomType.MaxPersonCount,
            roomType.TotalRooms,
            roomType.Services,
            roomType.Amenities
        );
    }
}