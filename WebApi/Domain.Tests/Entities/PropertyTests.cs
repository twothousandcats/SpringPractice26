using Domain.Entities;

namespace Domain.Tests.Entities;

public class PropertyTests
{
    [Fact]
    public void Constructor_ValidArguments_CreatesProperty()
    {
        // Arrange
        Guid id = Guid.NewGuid();

        // Act
        Property sut = new Property(
            id,
            "Test name",
            "Test country",
            "Test city",
            "Test address",
            10.10,
            20.20
        );

        // Assert
        Assert.Equal( id, sut.Id );
        Assert.Equal( "Test name", sut.Name );
        Assert.Equal( "Test country", sut.Country );
        Assert.Equal( "Test city", sut.City );
        Assert.Equal( "Test address", sut.Address );
        Assert.Equal( 10.10, sut.Latitude );
        Assert.Equal( 20.20, sut.Longitude );
    }

    [Fact]
    public void Constructor_EmptyId_Throws()
    {
        // Arrange
        // Act
        // Assert
        Assert.Throws<ArgumentException>( () => new Property(
                Guid.Empty,
                "Test name",
                "Test country",
                "Test city",
                "Test address",
                10.10,
                20.20
            )
        );
    }

    [Theory]
    [InlineData( "" )]
    [InlineData( "   " )]
    [InlineData( null )]
    public void Constructor_InvalidName_Throws( string? name )
    {
        // Arrange
        // Act
        // Assert
        Assert.Throws<ArgumentException>( () => new Property(
                Guid.NewGuid(),
                name!,
                "Test country",
                "Test city",
                "Test address",
                10.10,
                20.20
            )
        );
    }

    [Theory]
    [InlineData( "" )]
    [InlineData( "   " )]
    [InlineData( null )]
    public void Constructor_InvalidCountry_Throws( string? country )
    {
        // Arrange
        // Act
        // Assert
        Assert.Throws<ArgumentException>( () => new Property(
                Guid.NewGuid(),
                "Test name",
                country!,
                "Test city",
                "Test address",
                10.10,
                20.20
            )
        );
    }

    [Theory]
    [InlineData( "" )]
    [InlineData( "   " )]
    [InlineData( null )]
    public void Constructor_InvalidCity_Throws( string? city )
    {
        // Arrange
        // Act
        // Assert
        Assert.Throws<ArgumentException>( () => new Property(
                Guid.NewGuid(),
                "Test name",
                "Test country",
                city!,
                "Test address",
                10.10,
                20.20
            )
        );
    }

    [Theory]
    [InlineData( "" )]
    [InlineData( "   " )]
    [InlineData( null )]
    public void Constructor_InvalidAddress_Throws( string? address )
    {
        // Arrange
        // Act
        // Assert
        Assert.Throws<ArgumentException>( () => new Property(
                Guid.NewGuid(),
                "Test name",
                "Test country",
                "Test city",
                address!,
                10.10,
                20.20
            )
        );
    }

    [Theory]
    [InlineData( -90.01 )]
    [InlineData( 90.01 )]
    [InlineData( 180.0 )]
    public void Constructor_LatitudeOutOfRange_Throws( double latitude )
    {
        // Arrange
        // Act
        // Assert
        Assert.Throws<ArgumentOutOfRangeException>( () => new Property(
                Guid.NewGuid(),
                "Test name",
                "Test country",
                "Test city",
                "Test address",
                latitude,
                20.20
            )
        );
    }

    [Theory]
    [InlineData( -90.0 )]
    [InlineData( 90.0 )]
    [InlineData( 0.0 )]
    public void Constructor_LatitudeAtBoundary_DoesNotThrow( double latitude )
    {
        // Arrange, Act
        Property sut = new Property(
            Guid.NewGuid(),
            "Test name",
            "Test country",
            "Test city",
            "Test address",
            latitude,
            20.20
        );

        // Assert
        Assert.Equal( latitude, sut.Latitude );
    }

    [Theory]
    [InlineData( -180.01 )]
    [InlineData( 180.01 )]
    [InlineData( 360.0 )]
    public void Constructor_LongitudeOutOfRange_Throws( double longitude )
    {
        // Arrange
        // Act
        // Assert
        Assert.Throws<ArgumentOutOfRangeException>( () => new Property(
                Guid.NewGuid(),
                "Test name",
                "Test country",
                "Test city",
                "Test address",
                10.10,
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
        // Arrange, Act
        Property sut = new Property(
            Guid.NewGuid(),
            "Test name",
            "Test country",
            "Test city",
            "Test address",
            10.10,
            longitude
        );

        // Assert
        Assert.Equal( longitude, sut.Longitude );
    }

    [Fact]
    public void Update_ValidArguments_ChangesState()
    {
        // Arrange
        Property sut = new Property(
            Guid.NewGuid(),
            "Test name",
            "Test country",
            "Test city",
            "Test address",
            10.10,
            20.20
        );

        // Act
        sut.Update(
            "Test name2",
            "Test country2",
            "Test city2",
            "Test address2",
            30.30,
            40.40
        );

        // Assert
        Assert.Equal( "Test name2", sut.Name );
        Assert.Equal( "Test country2", sut.Country );
        Assert.Equal( "Test city2", sut.City );
        Assert.Equal( "Test address2", sut.Address );
        Assert.Equal( 30.30, sut.Latitude );
        Assert.Equal( 40.40, sut.Longitude );
    }

    [Fact]
    public void Update_InvalidArguments_DoesNotChangeState()
    {
        // Arrange
        Property sut = new Property(
            Guid.NewGuid(),
            "Test name",
            "Test country",
            "Test city",
            "Test address",
            10.10,
            20.20
        );

        string originalName = sut.Name;

        // Act
        Assert.Throws<ArgumentException>( () =>
            sut.Update(
                "",
                "Test country",
                "Test city",
                "Test address",
                10.10,
                20.20
            )
        );

        // Assert
        Assert.Equal( originalName, sut.Name );
    }

    [Fact]
    public void Update_LatitudeOutOfRange_Throws()
    {
        // Arrange
        Property sut = new Property(
            Guid.NewGuid(),
            "Test name",
            "Test country",
            "Test city",
            "Test address",
            10.10,
            20.20
        );

        // Act, Arrange
        Assert.Throws<ArgumentOutOfRangeException>( () =>
            sut.Update(
                "Test name2",
                "Test country2",
                "Test city2",
                "Test address2",
                91.30,
                40.40
            )
        );
    }
}