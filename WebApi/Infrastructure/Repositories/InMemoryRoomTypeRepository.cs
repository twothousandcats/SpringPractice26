using System.Collections.Concurrent;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;

namespace Infrastructure.Repositories;

public sealed class InMemoryRoomTypeRepository : IRoomTypeRepository
{
    private readonly ConcurrentDictionary<Guid, RoomType> _store = new ConcurrentDictionary<Guid, RoomType>();

    public void Add( RoomType roomType )
    {
        if ( !_store.TryAdd( roomType.Id, roomType ) )
        {
            throw new InvalidOperationException(
                $"RoomType  with id {roomType.Id} already exists"
            );
        }
    }

    public RoomType? Get( Guid id )
    {
        return _store.TryGetValue( id, out RoomType? roomType )
            ? roomType
            : null;
    }

    public IReadOnlyCollection<RoomType> GetByProperty( Guid propertyId )
    {
        return _store.Values
            .Where( roomType => roomType.PropertyId == propertyId )
            .ToArray();
    }

    public void Update( RoomType roomType )
    {
        if ( !_store.ContainsKey( roomType.Id ) )
        {
            throw new EntityNotFoundException( nameof( RoomType ), roomType.Id );
        }

        _store[ roomType.Id ] = roomType;
    }

    public bool Remove( Guid id )
    {
        return _store.TryRemove( id, out _ );
    }
}