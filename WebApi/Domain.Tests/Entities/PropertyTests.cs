using WebApi.Domain.Entities;
using Xunit;

namespace WebApi.Tests.Domain.Entities;

public class PropertyTests
{
    private const string HotelName = "Azimut";

    private const string HotelCountry = "Russia";

    private const string HotelCity = "Yoshkar-Ola";

    private const string HotelAddress = "Voskresensky Prospect, Building 11";

    private const double HotelLatitude = 56.63;

    private const double HotelLongitude = 47.91;

    private static Property CreateValidProperty(
        Guid id,
        string hotelName = HotelName,
        string hotelCountry = HotelCountry,
        string hotelCity = HotelCity,
        string hotelAddress = HotelAddress,
        double hotelLatitude = HotelLatitude,
        double hotelLongitude = HotelLongitude
    ) => new Property(
        id,
        hotelName,
        hotelCountry,
        hotelCity,
        hotelAddress,
        hotelLatitude,
        hotelLongitude
    );

    [Fact]
    public void Constructor_ValidArguments_CreatesProperty()
    {
        Guid id = Guid.NewGuid();

        Property property = CreateValidProperty( id );

        Assert.Equal( id, property.Id );
        Assert.Equal( HotelName, property.Name );
        Assert.Equal( HotelCountry, property.Country );
        Assert.Equal( HotelCity, property.City );
        Assert.Equal( HotelAddress, property.Address );
        Assert.Equal( HotelLatitude, property.Latitude );
        Assert.Equal( HotelLongitude, property.Longitude );
    }

    [Fact]
    public void Constructor_EmptyId_Throws()
    {
        Assert.Throws<ArgumentException>( () => new Property(
                Guid.Empty,
                HotelName,
                HotelCountry,
                HotelCity,
                HotelAddress,
                HotelLatitude,
                HotelLongitude
            )
        );
    }

    [Theory]
    [InlineData( "" )]
    [InlineData( "   " )]
    [InlineData( null )]
    public void Constructor_InvalidName_Throws( string? name )
    {
        Assert.Throws<ArgumentException>( () => CreateValidProperty( Guid.NewGuid(), name! ) );
    }

    [Theory]
    [InlineData( "" )]
    [InlineData( "   " )]
    [InlineData( null )]
    public void Constructor_InvalidCountry_Throws( string? country )
    {
        Assert.Throws<ArgumentException>( () => new Property(
                Guid.NewGuid(),
                HotelName,
                country!,
                HotelCity,
                HotelAddress,
                HotelLatitude,
                HotelLongitude
            )
        );
    }

    [Theory]
    [InlineData( "" )]
    [InlineData( "   " )]
    [InlineData( null )]
    public void Constructor_InvalidCity_Throws( string? city )
    {
        Assert.Throws<ArgumentException>( () => new Property(
                Guid.NewGuid(),
                HotelName,
                HotelCountry,
                city!,
                HotelAddress,
                HotelLatitude,
                HotelLongitude
            )
        );
    }

    [Theory]
    [InlineData( "" )]
    [InlineData( "   " )]
    [InlineData( null )]
    public void Constructor_InvalidAddress_Throws( string? address )
    {
        Assert.Throws<ArgumentException>( () => new Property(
                Guid.NewGuid(),
                HotelName,
                HotelCountry,
                HotelCity,
                address!,
                HotelLatitude,
                HotelLongitude
            )
        );
    }

    [Theory]
    [InlineData( -90.01 )]
    [InlineData( 90.01 )]
    [InlineData( 180.0 )]
    public void Constructor_LatitudeOutOfRange_Throws( double latitude )
    {
        Assert.Throws<ArgumentOutOfRangeException>( () => new Property(
                Guid.NewGuid(),
                HotelName,
                HotelCountry,
                HotelCity,
                HotelAddress,
                latitude,
                HotelLongitude
            )
        );
    }

    [Theory]
    [InlineData( -90.0 )]
    [InlineData( 90.0 )]
    [InlineData( 0.0 )]
    public void Constructor_LatitudeAtBoundary_DoesNotThrow( double latitude )
    {
        Property property = CreateValidProperty(
            Guid.NewGuid(),
            HotelName,
            HotelCountry,
            HotelCity,
            HotelAddress,
            latitude,
            HotelLongitude
        );

        Assert.Equal( latitude, property.Latitude );
    }

    [Theory]
    [InlineData( -180.01 )]
    [InlineData( 180.01 )]
    [InlineData( 360.0 )]
    public void Constructor_LongitudeOutOfRange_Throws( double longitude )
    {
        Assert.Throws<ArgumentOutOfRangeException>( () => CreateValidProperty(
                Guid.NewGuid(),
                HotelName,
                HotelCountry,
                HotelCity,
                HotelAddress,
                HotelLatitude,
                longitude
            )
        );
    }

    [Theory]
    [InlineData( -180.0 )]
    [InlineData( 180.0 )]
    [InlineData( 0.0 )]
    public void Constructor_LongitudeAtBoundary_DoesNotThrow( double longitude )
    {
        Property property = CreateValidProperty(
            Guid.NewGuid(),
            HotelName,
            HotelCountry,
            HotelCity,
            HotelAddress,
            HotelLatitude,
            longitude
        );

        Assert.Equal( longitude, property.Longitude );
    }

    [Fact]
    public void Update_ValidArguments_ChangesState()
    {
        Property property = CreateValidProperty( Guid.NewGuid() );

        property.Update(
            "Carlton",
            "Russia",
            "Moscow",
            "Tverskaya Street, 3",
            61.5,
            23.76
        );

        Assert.Equal( "Carlton", property.Name );
        Assert.Equal( "Russia", property.Country );
        Assert.Equal( "Moscow", property.City );
        Assert.Equal( "Tverskaya Street, 3", property.Address );
        Assert.Equal( 61.5, property.Latitude );
        Assert.Equal( 23.76, property.Longitude );
    }

    [Fact]
    public void Update_InvalidArguments_DoesNotChangeState()
    {
        Property property = CreateValidProperty( Guid.NewGuid() );
        string originalName = property.Name;

        Assert.Throws<ArgumentException>( () =>
            property.Update(
                "",
                "Russia",
                "Moscow",
                "Tverskaya Street, 3",
                61.5,
                23.76
            )
        );

        Assert.Equal( originalName, property.Name );
    }

    [Fact]
    public void Update_LatitudeOutOfRange_Throws()
    {
        Property property = CreateValidProperty( Guid.NewGuid() );

        Assert.Throws<ArgumentOutOfRangeException>( () =>
            property.Update(
                "Carlton",
                "Russia",
                "Moscow",
                "Tverskaya Street, 3",
                91.0,
                23.76
            )
        );
    }
}