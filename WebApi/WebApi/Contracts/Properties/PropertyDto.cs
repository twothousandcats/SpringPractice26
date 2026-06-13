namespace WebApi.Contracts.Properties;

public sealed record PropertyDto(
    Guid Id,
    string Name,
    string Country,
    string City,
    string Address,
    double Latitude,
    double Longitude
);