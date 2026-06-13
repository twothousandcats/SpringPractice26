using Domain.Services;
using WebApi.Contracts.Common;
using WebApi.Contracts.Properties;
using WebApi.Contracts.RoomTypes;

namespace WebApi.Contracts.Search;

public static class SearchVariantMappings
{
    public static SearchVariantDto ToDto( this SearchVariant variant )
    {
        return new SearchVariantDto(
            variant.Property.ToDto(),
            variant.RoomType.ToDto(),
            variant.TotalForPeriod.ToDto()
        );
    }
}