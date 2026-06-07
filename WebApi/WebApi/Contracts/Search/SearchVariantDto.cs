using WebApi.Contracts.Common;
using WebApi.Contracts.Properties;
using WebApi.Contracts.RoomTypes;

namespace WebApi.Contracts.Search;

public record SearchVariantDto(
    PropertyDto Property,
    RoomTypeDto RoomType,
    MoneyDto TotalForPeriod
);