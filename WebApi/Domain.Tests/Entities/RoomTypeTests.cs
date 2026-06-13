using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Tests.Entities;

public class RoomTypeTests
{
    [Fact]
    public void Constructor_ValidArguments_CreatesRoomType()
    {
        // Arrange
        Guid id = Guid.NewGuid();
        Guid propId = Guid.NewGuid();
        Money price = new Money( 100m, "Test currency" );

        // Act
        RoomType sut = new RoomType(
            id,
            propId,
            "Test name",
            price,
            1,
            2,
            2,
            new[] { "Breakfast" },
            new[] { "Wi-Fi", "TV" }
        );

        // Assert
        Assert.Equal( id, sut.Id );
        Assert.Equal( propId, sut.PropertyId );
        Assert.Equal( "Test name", sut.Name );
        Assert.Equal( price, sut.DailyPrice );
        Assert.Equal( 1, sut.MinPersonCount );
        Assert.Equal( 2, sut.MaxPersonCount );
        Assert.Equal( 2, sut.TotalRooms );
        Assert.Equal( new[] { "Breakfast" }, sut.Services );
        Assert.Equal( new[] { "Wi-Fi", "TV" }, sut.Amenities );
    }

    [Fact]
    public void Constructor_EmptyId_Throws()
    {
        // Arrange
        // Act
        // Assert
        Assert.Throws<ArgumentException>( () => new RoomType(
                Guid.Empty,
                Guid.NewGuid(),
                "Test name",
                new Money( 100m, "Test currency" ),
                1,
                2,
                2,
                Array.Empty<string>(),
                Array.Empty<string>()
            )
        );
    }

    [Fact]
    public void Constructor_EmptyPropertyId_Throws()
    {
        // Arrange
        // Act
        // Assert
        Assert.Throws<ArgumentException>( () => new RoomType(
                Guid.NewGuid(),
                Guid.Empty,
                "Test name",
                new Money( 100m, "Test currency" ),
                1,
                2,
                2,
                Array.Empty<string>(),
                Array.Empty<string>()
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
        Assert.Throws<ArgumentException>( () => new RoomType(
                Guid.NewGuid(),
                Guid.NewGuid(),
                name!,
                new Money( 100m, "Test currency" ),
                1,
                2,
                2,
                Array.Empty<string>(),
                Array.Empty<string>()
            )
        );
    }

    [Fact]
    public void Constructor_NullDailyPrice_Throws()
    {
        // Arrange
        // Act
        // Assert
        Assert.Throws<ArgumentNullException>( () => new RoomType(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Test name",
                null!,
                1,
                2,
                2,
                Array.Empty<string>(),
                Array.Empty<string>()
            )
        );
    }

    [Fact]
    public void Constructor_ZeroDailyPrice_Throws()
    {
        // Arrange
        // Act
        // Assert
        Assert.Throws<ArgumentOutOfRangeException>( () => new RoomType(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Test name",
                new Money( 0m, "Test currency" ),
                1,
                2,
                2,
                Array.Empty<string>(),
                Array.Empty<string>()
            )
        );
    }

    [Theory]
    [InlineData( 0 )]
    [InlineData( -1 )]
    public void Constructor_NonPositiveMinPersonCount_Throws( int minPersonCount )
    {
        // Arrange
        // Act
        // Assert
        Assert.Throws<ArgumentOutOfRangeException>( () => new RoomType(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Test name",
                new Money( 100m, "Test currency" ),
                minPersonCount,
                2,
                2,
                Array.Empty<string>(),
                Array.Empty<string>()
            )
        );
    }

    [Fact]
    public void Constructor_MaxLessThanMin_Throws()
    {
        // Arrange
        // Act
        // Assert
        Assert.Throws<ArgumentException>( () => new RoomType(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Test name",
                new Money( 100m, "Test currency" ),
                2,
                1,
                2,
                Array.Empty<string>(),
                Array.Empty<string>()
            )
        );
    }

    [Fact]
    public void Constructor_MaxEqualsMin_DoesNotThrow()
    {
        // Arrange
        int minPersonCount = 2;
        int maxPersonCount = 2;

        // Act
        RoomType roomType = new RoomType(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Test name",
            new Money( 100m, "Test currency" ),
            minPersonCount,
            maxPersonCount,
            2,
            Array.Empty<string>(),
            Array.Empty<string>()
        );

        // Arrange
        Assert.Equal( 2, roomType.MinPersonCount );
        Assert.Equal( 2, roomType.MaxPersonCount );
    }

    [Theory]
    [InlineData( 0 )]
    [InlineData( -1 )]
    public void Constructor_NonPositiveRoomsCount_Throws( int roomsCount )
    {
        // Arrange
        // Act
        // Assert
        Assert.Throws<ArgumentOutOfRangeException>( () => new RoomType(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Test name",
                new Money( 100m, "Test currency" ),
                1,
                2,
                roomsCount,
                Array.Empty<string>(),
                Array.Empty<string>()
            )
        );
    }

    [Fact]
    public void Constructor_NullServices_Throws()
    {
        // Arrange
        // Act
        // Assert
        Assert.Throws<ArgumentNullException>( () => new RoomType(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Test name",
                new Money( 100m, "Test currency" ),
                1,
                2,
                2,
                null!,
                Array.Empty<string>()
            )
        );
    }

    [Fact]
    public void Constructor_ServicesContainEmpty_Throws()
    {
        // Arrange
        // Act
        // Assert
        Assert.Throws<ArgumentException>( () => new RoomType(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Test name",
                new Money( 100m, "Test currency" ),
                1,
                2,
                2,
                new[] { "Breakfast", string.Empty },
                Array.Empty<string>()
            )
        );
    }

    [Fact]
    public void Constructor_AmenitiesContainEmpty_Throws()
    {
        // Arrange
        // Act
        // Assert
        Assert.Throws<ArgumentException>( () => new RoomType(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Test name",
                new Money( 100m, "Test currency" ),
                1,
                2,
                2,
                Array.Empty<string>(),
                new[] { "Wi-Fi", "   " }
            )
        );
    }

    [Fact]
    public void Constructor_ServicesCopiedDefensively()
    {
        // Arrange
        List<string> services = new List<string>
        {
            "Breakfast",
        };

        // Act
        RoomType sut = new RoomType(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Test name",
            new Money( 100m, "Test currency" ),
            1,
            2,
            2,
            services,
            new[] { "Wi-Fi" }
        );

        // Assert
        Assert.Single( sut.Services );
    }

    [Theory]
    [InlineData( 1, true )]
    [InlineData( 2, true )]
    [InlineData( 3, false )]
    [InlineData( 0, false )]
    public void Fits_ReturnsExpected( int guests, bool expected )
    {
        // Arrange
        RoomType sut = new RoomType(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Test name",
            new Money( 100m, "Test currency" ),
            1,
            2,
            2,
            new[] { "Breakfast" },
            new[] { "Wi-Fi" }
        );

        // Act
        bool result = sut.IsFits( guests );

        // Assert
        Assert.Equal( expected, result );
    }

    [Fact]
    public void Update_ValidArguments_ChangesState()
    {
        // Arrange
        Money newPrice = new Money( 200m, "New currency" );
        RoomType sut = new RoomType(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Test name",
            new Money( 100m, "Test currency" ),
            1,
            2,
            2,
            new[] { "Breakfast" },
            new[] { "Wi-Fi" }
        );

        // Act
        sut.Update(
            "Deluxe",
            newPrice,
            2,
            4,
            10,
            new[] { "Breakfast", "Spa" },
            new[] { "Wi-Fi" }
        );

        // Assert
        Assert.Equal( "Deluxe", sut.Name );
        Assert.Equal( newPrice, sut.DailyPrice );
        Assert.Equal( 2, sut.MinPersonCount );
        Assert.Equal( 4, sut.MaxPersonCount );
        Assert.Equal( 10, sut.TotalRooms );
        Assert.Equal( new[] { "Breakfast", "Spa" }, sut.Services );
        Assert.Equal( new[] { "Wi-Fi" }, sut.Amenities );
    }
}