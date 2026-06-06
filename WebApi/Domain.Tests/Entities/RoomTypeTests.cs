using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Tests.Entities;

public class RoomTypeTests
{
    private const string DefaultCurrency = "EUR";

    const string RoomTypeName = "Standard Double";

    const int MinPersonCount = 1;

    const int MaxPersonCount = 2;

    const int RoomsCount = 5;

    private static RoomType CreateValidRoomType() => new RoomType(
        Guid.NewGuid(),
        Guid.NewGuid(),
        RoomTypeName,
        new Money( 120m, DefaultCurrency ),
        MinPersonCount,
        MaxPersonCount,
        RoomsCount,
        new[] { "Breakfast" },
        new[] { "Wi-Fi", "TV" }
    );

    [Fact]
    public void Constructor_ValidArguments_CreatesRoomType()
    {
        Guid id = Guid.NewGuid();
        Guid propId = Guid.NewGuid();
        Money price = new Money( 120m, DefaultCurrency );

        RoomType roomType = new RoomType(
            id,
            propId,
            RoomTypeName,
            price,
            MinPersonCount,
            MaxPersonCount,
            RoomsCount,
            new[] { "Breakfast" },
            new[] { "Wi-Fi", "TV" }
        );

        Assert.Equal( id, roomType.Id );
        Assert.Equal( propId, roomType.PropertyId );
        Assert.Equal( RoomTypeName, roomType.Name );
        Assert.Equal( price, roomType.DailyPrice );
        Assert.Equal( MinPersonCount, roomType.MinPersonCount );
        Assert.Equal( MaxPersonCount, roomType.MaxPersonCount );
        Assert.Equal( RoomsCount, roomType.TotalRooms );
        Assert.Equal( new[] { "Breakfast" }, roomType.Services );
        Assert.Equal( new[] { "Wi-Fi", "TV" }, roomType.Amenities );
    }

    [Fact]
    public void Constructor_EmptyId_Throws()
    {
        Assert.Throws<ArgumentException>( () => new RoomType(
                Guid.Empty,
                Guid.NewGuid(),
                RoomTypeName,
                new Money( 120m, DefaultCurrency ),
                MinPersonCount,
                MaxPersonCount,
                RoomsCount,
                Array.Empty<string>(),
                Array.Empty<string>()
            )
        );
    }

    [Fact]
    public void Constructor_EmptyPropertyId_Throws()
    {
        Assert.Throws<ArgumentException>( () => new RoomType(
                Guid.NewGuid(),
                Guid.Empty,
                RoomTypeName,
                new Money( 120m, DefaultCurrency ),
                MinPersonCount,
                MaxPersonCount,
                RoomsCount,
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
        Assert.Throws<ArgumentException>( () => new RoomType(
                Guid.NewGuid(),
                Guid.NewGuid(),
                name!,
                new Money( 120m, DefaultCurrency ),
                MinPersonCount,
                MaxPersonCount,
                RoomsCount,
                Array.Empty<string>(),
                Array.Empty<string>()
            )
        );
    }

    [Fact]
    public void Constructor_NullDailyPrice_Throws()
    {
        Assert.Throws<ArgumentNullException>( () => new RoomType(
                Guid.NewGuid(),
                Guid.NewGuid(),
                RoomTypeName,
                null!,
                MinPersonCount,
                MaxPersonCount,
                RoomsCount,
                Array.Empty<string>(),
                Array.Empty<string>()
            )
        );
    }

    [Fact]
    public void Constructor_ZeroDailyPrice_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>( () => new RoomType(
                Guid.NewGuid(),
                Guid.NewGuid(),
                RoomTypeName,
                new Money( 0m, DefaultCurrency ),
                MinPersonCount,
                MaxPersonCount,
                RoomsCount,
                Array.Empty<string>(),
                Array.Empty<string>()
            )
        );
    }

    [Theory]
    [InlineData( 0 )]
    [InlineData( -1 )]
    public void Constructor_NonPositiveMinPersonCount_Throws( int min )
    {
        Assert.Throws<ArgumentOutOfRangeException>( () => new RoomType(
                Guid.NewGuid(),
                Guid.NewGuid(),
                RoomTypeName,
                new Money( 120m, DefaultCurrency ),
                min,
                MaxPersonCount,
                RoomsCount,
                Array.Empty<string>(),
                Array.Empty<string>()
            )
        );
    }

    [Fact]
    public void Constructor_MaxLessThanMin_Throws()
    {
        Assert.Throws<ArgumentException>( () => new RoomType(
                Guid.NewGuid(),
                Guid.NewGuid(),
                RoomTypeName,
                new Money( 120m, DefaultCurrency ),
                2,
                1,
                RoomsCount,
                Array.Empty<string>(),
                Array.Empty<string>()
            )
        );
    }

    [Fact]
    public void Constructor_MaxEqualsMin_DoesNotThrow()
    {
        RoomType roomType = new RoomType(
            Guid.NewGuid(),
            Guid.NewGuid(),
            RoomTypeName,
            new Money( 120m, DefaultCurrency ),
            MinPersonCount,
            MinPersonCount,
            RoomsCount,
            Array.Empty<string>(),
            Array.Empty<string>()
        );

        Assert.Equal( MinPersonCount, roomType.MinPersonCount );
        Assert.Equal( MinPersonCount, roomType.MaxPersonCount );
    }

    [Theory]
    [InlineData( 0 )]
    [InlineData( -1 )]
    public void Constructor_NonPositiveRoomsCount_Throws( int roomsCount )
    {
        Assert.Throws<ArgumentOutOfRangeException>( () => new RoomType(
                Guid.NewGuid(),
                Guid.NewGuid(),
                RoomTypeName,
                new Money( 120m, DefaultCurrency ),
                MinPersonCount,
                MaxPersonCount,
                roomsCount,
                Array.Empty<string>(),
                Array.Empty<string>()
            )
        );
    }

    [Fact]
    public void Constructor_NullServices_Throws()
    {
        Assert.Throws<ArgumentNullException>( () => new RoomType(
                Guid.NewGuid(),
                Guid.NewGuid(),
                RoomTypeName,
                new Money( 120m, DefaultCurrency ),
                MinPersonCount,
                MinPersonCount,
                RoomsCount,
                null!,
                Array.Empty<string>()
            )
        );
    }

    [Fact]
    public void Constructor_ServicesContainEmpty_Throws()
    {
        Assert.Throws<ArgumentException>( () => new RoomType(
                Guid.NewGuid(),
                Guid.NewGuid(),
                RoomTypeName,
                new Money( 120m, DefaultCurrency ),
                MinPersonCount,
                MinPersonCount,
                RoomsCount,
                new[] { "Breakfast", string.Empty },
                Array.Empty<string>()
            )
        );
    }

    [Fact]
    public void Constructor_AmenitiesContainEmpty_Throws()
    {
        Assert.Throws<ArgumentException>( () => new RoomType(
                Guid.NewGuid(),
                Guid.NewGuid(),
                RoomTypeName,
                new Money( 120m, DefaultCurrency ),
                MinPersonCount,
                MinPersonCount,
                RoomsCount,
                Array.Empty<string>(),
                new[] { "Wi-Fi", "   " }
            )
        );
    }

    [Fact]
    public void Constructor_ServicesCopiedDefensively()
    {
        List<string> services = new List<string>
        {
            "Breakfast",
        };

        RoomType roomType = new RoomType(
            Guid.NewGuid(),
            Guid.NewGuid(),
            RoomTypeName,
            new Money( 120m, DefaultCurrency ),
            MinPersonCount,
            MaxPersonCount,
            RoomsCount,
            services,
            Array.Empty<string>()
        );

        Assert.Single( roomType.Services );
    }

    [Theory]
    [InlineData( 1, true )]
    [InlineData( 2, true )]
    [InlineData( 3, false )]
    [InlineData( 0, false )]
    public void Fits_ReturnsExpected( int guests, bool expected )
    {
        RoomType roomType = CreateValidRoomType();

        bool result = roomType.IsFits( guests );

        Assert.Equal( expected, result );
    }

    [Fact]
    public void Update_ValidArguments_ChangesState()
    {
        RoomType roomType = CreateValidRoomType();
        Money newPrice = new Money( 200m, DefaultCurrency );

        roomType.Update(
            "Deluxe",
            newPrice,
            2,
            4,
            10,
            new[] { "Breakfast", "Spa" },
            new[] { "Wi-Fi" }
        );

        Assert.Equal( "Deluxe", roomType.Name );
        Assert.Equal( newPrice, roomType.DailyPrice );
        Assert.Equal( 2, roomType.MinPersonCount );
        Assert.Equal( 4, roomType.MaxPersonCount );
        Assert.Equal( 10, roomType.TotalRooms );
        Assert.Equal( new[] { "Breakfast", "Spa" }, roomType.Services );
        Assert.Equal( new[] { "Wi-Fi" }, roomType.Amenities );
    }
}