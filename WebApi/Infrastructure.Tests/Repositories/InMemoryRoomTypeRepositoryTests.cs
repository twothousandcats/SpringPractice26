using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Domain.ValueObjects;
using Infrastructure.Repositories;

namespace Infrastructure.Tests.Repositories;

public class InMemoryRoomTypeRepositoryTests
{
    [Fact]
    public void Add_Then_Get_ReturnsSameInstance()
    {
        // Arrange
        RoomType roomType = new RoomType(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Test name",
            new Money( 100m, "Test currency" ),
            1,
            2,
            2,
            Array.Empty<string>(),
            Array.Empty<string>()
        );

        InMemoryRoomTypeRepository sut = new InMemoryRoomTypeRepository();
        sut.Add( roomType );

        // Act
        RoomType? loaded = sut.Get( roomType.Id );

        // Assert
        Assert.Same( roomType, loaded );
    }

    [Fact]
    public void Add_Duplicate_Throws()
    {
        // Arrange
        RoomType roomType = new RoomType(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Test name",
            new Money( 100m, "Test currency" ),
            1,
            2,
            2,
            Array.Empty<string>(),
            Array.Empty<string>()
        );

        InMemoryRoomTypeRepository sut = new InMemoryRoomTypeRepository();
        sut.Add( roomType );

        // Act, Assert
        Assert.Throws<InvalidOperationException>( () => sut.Add( roomType ) );
    }

    [Fact]
    public void GetByProperty_FiltersByPropertyId()
    {
        // Arrange
        Guid firstPropertyId = Guid.NewGuid();
        Guid secondPropertyId = Guid.NewGuid();
        RoomType firstRoomTypeA = new RoomType(
            Guid.NewGuid(),
            firstPropertyId,
            "Test name",
            new Money( 100m, "Test currency" ),
            1,
            2,
            2,
            Array.Empty<string>(),
            Array.Empty<string>()
        );

        RoomType secondRoomTypeA = new RoomType(
            Guid.NewGuid(),
            firstPropertyId,
            "Test name2",
            new Money( 100m, "Test currency" ),
            1,
            2,
            2,
            Array.Empty<string>(),
            Array.Empty<string>()
        );

        RoomType firstRoomTypeB = new RoomType(
            Guid.NewGuid(),
            secondPropertyId,
            "Test name",
            new Money( 100m, "Test currency" ),
            1,
            2,
            2,
            Array.Empty<string>(),
            Array.Empty<string>()
        );

        InMemoryRoomTypeRepository sut = new InMemoryRoomTypeRepository();
        sut.Add( firstRoomTypeA );
        sut.Add( firstRoomTypeB );
        sut.Add( secondRoomTypeA );

        // Act
        IReadOnlyCollection<RoomType> result = sut.GetByProperty( firstPropertyId );

        // Assert
        Assert.Equal( 2, result.Count );
        Assert.Contains( firstRoomTypeA, result );
        Assert.Contains( secondRoomTypeA, result );
    }

    [Fact]
    public void Update_Unknown_ThrowsEntityNotFound()
    {
        // Arrange
        RoomType roomType = new RoomType(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Test name",
            new Money( 100m, "Test currency" ),
            1,
            2,
            2,
            Array.Empty<string>(),
            Array.Empty<string>()
        );

        InMemoryRoomTypeRepository sut = new InMemoryRoomTypeRepository();

        // Act, Assert
        Assert.Throws<EntityNotFoundException>( () => sut.Update( roomType ) );
    }

    [Fact]
    public void Remove_Existing_ReturnsTrue()
    {
        // Arrange
        RoomType roomType = new RoomType(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Test name",
            new Money( 100m, "Test currency" ),
            1,
            2,
            2,
            Array.Empty<string>(),
            Array.Empty<string>()
        );

        InMemoryRoomTypeRepository sut = new InMemoryRoomTypeRepository();
        sut.Add( roomType );

        // Act
        bool result = sut.Remove( roomType.Id );

        Assert.True( result );
        Assert.Null( sut.Get( roomType.Id ) );
    }

    [Fact]
    public void Remove_Unknown_ReturnsFalse()
    {
        // Arrange
        InMemoryRoomTypeRepository sut = new InMemoryRoomTypeRepository();

        // Act
        bool result = sut.Remove( Guid.NewGuid() );

        // Assert
        Assert.False( result );
    }
}