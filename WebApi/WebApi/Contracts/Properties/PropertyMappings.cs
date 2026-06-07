using Domain.Entities;

namespace WebApi.Contracts.Properties;

public static class PropertyMappings
{
    public static PropertyDto ToDto( this Property property )
    {
        return new PropertyDto(
            property.Id,
            property.Name,
            property.Country,
            property.City,
            property.Address,
            property.Latitude,
            property.Longitude
        );
    }
}