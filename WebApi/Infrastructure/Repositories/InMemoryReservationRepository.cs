using System.Collections.Concurrent;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Domain.ValueObjects;

namespace Infrastructure.Repositories;

public sealed class InMemoryReservationRepository : IReservationRepository
{
    private readonly ConcurrentDictionary<Guid, Reservation> _store = new ConcurrentDictionary<Guid, Reservation>();

    public void Add( Reservation reservation )
    {
        if ( !_store.TryAdd( reservation.Id, reservation ) )
        {
            throw new InvalidOperationException(
                $"Reservation with id {reservation.Id} already exists"
            );
        }
    }

    public Reservation? Get( Guid id )
    {
        return _store.TryGetValue( id, out Reservation? reservation )
            ? reservation
            : null;
    }

    public IReadOnlyCollection<Reservation> Search()
    {
        return _store.Values.ToArray();
    }

    public IReadOnlyCollection<Reservation> GetOverlapping( Guid roomTypeId, DateRange period )
    {
        return _store.Values
            .Where( reservation => reservation.RoomTypeId == roomTypeId && reservation.Period.Overlaps( period ) )
            .ToArray();
    }

    public void Update( Reservation reservation )
    {
        if ( !_store.ContainsKey( reservation.Id ) )
        {
            throw new EntityNotFoundException(
                nameof( Reservation ),
                reservation.Id
            );
        }

        _store[ reservation.Id ] = reservation;
    }
}