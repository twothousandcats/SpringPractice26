using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Repositories;

public interface IReservationRepository
{
    void Add( Reservation reservation );
    IReadOnlyCollection<Reservation> Search();
    IReadOnlyCollection<Reservation> GetOverlapping( Guid roomTypeId, DateRange period );
    void Update( RoomType roomType );
}