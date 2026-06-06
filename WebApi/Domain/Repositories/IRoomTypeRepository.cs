using Domain.Entities;

namespace Domain.Repositories;

public interface IRoomTypeRepository
{
    void Add( RoomType roomType );
    RoomType? Get( Guid id );
    IReadOnlyCollection<RoomType> GetByProperty( Guid propertyId );
    void Update( RoomType roomType );
    void Remove( Guid id );
}