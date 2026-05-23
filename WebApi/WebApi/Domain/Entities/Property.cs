namespace WebApi.Domain.Entities;

public sealed class Property
{
    private const double MinLatitude = -90.0;

    private const double MaxLatitude = 90.0;

    private const double MinLongitude = -180.0;

    private const double MaxLongitude = 180.0;

    public Guid Id { get; }

    public string Name { get; private set; }

    public string Country { get; private set; }

    public string City { get; private set; }

    public string Address { get; private set; }

    public double Latitude { get; private set; }

    public double Longitude { get; private set; }

    public Property(
        Guid id,
        string name,
        string country,
        string city,
        string address,
        double latitude,
        double longitude
    )
    {
        if ( id == Guid.Empty )
        {
            throw new ArgumentException( "Id is required", nameof( id ) );
        }

        Validate( name, country, city, address, latitude, longitude );
        Id = id;
        Name = name;
        Country = country;
        City = city;
        Address = address;
        Latitude = latitude;
        Longitude = longitude;
    }

    public void Update(
        string name,
        string country,
        string city,
        string address,
        double latitude,
        double longitude
    )
    {
        Validate( name, country, city, address, latitude, longitude );

        Name = name;
        Country = country;
        City = city;
        Address = address;
        Latitude = latitude;
        Longitude = longitude;
    }

    private static void Validate(
        string name,
        string country,
        string city,
        string address,
        double latitude,
        double longitude
    )
    {
        if ( string.IsNullOrWhiteSpace( name ) )
        {
            throw new ArgumentException( "Name is required", nameof( name ) );
        }

        if ( string.IsNullOrWhiteSpace( country ) )
        {
            throw new ArgumentException( "Country is required", nameof( country ) );
        }

        if ( string.IsNullOrWhiteSpace( city ) )
        {
            throw new ArgumentException( "City is required", nameof( city ) );
        }

        if ( string.IsNullOrWhiteSpace( address ) )
        {
            throw new ArgumentException( "Address is required", nameof( address ) );
        }

        if ( latitude is < MinLatitude or > MaxLatitude )
        {
            throw new ArgumentOutOfRangeException( nameof( latitude ) );
        }

        if ( longitude is < MinLongitude or > MaxLongitude )
        {
            throw new ArgumentOutOfRangeException( nameof( longitude ) );
        }
    }
}