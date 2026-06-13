namespace WebApi.Contracts.Properties;

public sealed record UpdatePropertyRequest(
    string Name,
    string Country,
    string City,
    string Address,
    double Latitude,
    double Longitude
);