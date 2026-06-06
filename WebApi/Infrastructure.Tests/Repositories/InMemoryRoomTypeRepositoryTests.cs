using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Domain.ValueObjects;
using Infrastructure.Repositories;

namespace Infrastructure.Tests.Repositories;

public class InMemoryRoomTypeRepositoryTests
{
    private const string Currency = "EUR";

    private readonly IRoomTypeRepository _repository = new InMemoryRoomTypeRepository();

    private static RoomType CreateRoomType( Guid propertyId )
    {
        return new RoomType(
            Guid.NewGuid(),
            propertyId,
            "Standard",
            new Money( 100m, Currency ),
            1,
            2,
            2,
            Array.Empty<string>(),
            Array.Empty<string>()
        );
    }

    [Fact]
    public void Add_Then_Get_ReturnsSameInstance()
    {
        RoomType roomType = CreateRoomType( Guid.NewGuid() );

        _repository.Add( roomType );
        RoomType? loaded = _repository.Get( roomType.Id );

        Assert.Same( roomType, loaded );
    }

    [Fact]
    public void Add_Duplicate_Throws()
    {
        RoomType roomType = CreateRoomType( Guid.NewGuid() );
        _repository.Add( roomType );

        Assert.Throws<InvalidOperationException>( () => _repository.Add( roomType ) );
    }

    [Fact]
    public void GetByProperty_FiltersByPropertyId()
    {
        Guid propertyA = Guid.NewGuid();
        Guid propertyB = Guid.NewGuid();
        RoomType firstA = CreateRoomType( propertyA );
        RoomType secondA = CreateRoomType( propertyA );
        RoomType single = CreateRoomType( propertyB );
        _repository.Add( firstA );
        _repository.Add( secondA );
        _repository.Add( single );

        IReadOnlyCollection<RoomType> result = _repository.GetByProperty( propertyA );

        Assert.Equal( 2, result.Count );
        Assert.Contains( firstA, result );
        Assert.Contains( secondA, result );
    }

    [Fact]
    public void Update_Unknown_ThrowsEntityNotFound()
    {
        RoomType roomType = CreateRoomType( Guid.NewGuid() );

        Assert.Throws<EntityNotFoundException>( () => _repository.Update( roomType ) );
    }

    [Fact]
    public void Remove_Existing_ReturnsTrue()
    {
        RoomType roomType = CreateRoomType( Guid.NewGuid() );
        _repository.Add( roomType );

        Assert.True( _repository.Remove( roomType.Id ) );
        Assert.Null( _repository.Get( roomType.Id ) );
    }

    [Fact]
    public void Remove_Unknown_ReturnsFalse()
    {
        Assert.False( _repository.Remove( Guid.NewGuid() ) );
    }
}