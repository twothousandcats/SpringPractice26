namespace WebApi.Contracts.Properties;

public sealed record CreatePropertyRequest(
    string Name,
    string Country,
    string City,
    string Address,
    double Latitude,
    double Longitude
);