using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Repositories;

public interface IReservationRepository
{
    void Add( Reservation reservation );
    Reservation? Get( Guid id );
    IReadOnlyCollection<Reservation> Search();
    IReadOnlyCollection<Reservation> GetOverlapping( Guid roomTypeId, DateRange period );
    void Update( Reservation reservation );
}