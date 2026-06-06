using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Repositories;

public interface IReservationRepository
{
    void Add( Reservation reservation );
    IReadOnlyCollection<Reservation> List();
    IReadOnlyCollection<Reservation> ListOverlapping( Guid roomTypeId, DateRange period );
    void Update( RoomType roomType );
}