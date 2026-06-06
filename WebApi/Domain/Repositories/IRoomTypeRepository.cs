using Domain.Entities;

namespace Domain.Repositories;

public interface IRoomTypeRepository
{
    void Add( RoomType roomType );
    RoomType? Get( Guid id );
    IReadOnlyCollection<RoomType> ListByProperty( Guid propertyId );
    void Update( RoomType roomType );
    void Remove( Guid id );
}