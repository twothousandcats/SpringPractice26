using Domain.Entities;
using Domain.Repositories;
using Domain.ValueObjects;

namespace Domain.Services;

public sealed class AvailabilityChecker : IAvailabilityChecker
{
    private readonly IReservationRepository _reservationRepository;

    public AvailabilityChecker( IReservationRepository repository )
    {
        _reservationRepository = repository;
    }

    public bool CanBook( RoomType roomType, DateRange period )
    {
        IReadOnlyCollection<Reservation> overlaping = _reservationRepository.GetOverlapping( roomType.Id, period );
        int activeOverlap = overlaping.Count( reservation => reservation.Status == ReservationStatus.Active );

        return activeOverlap < roomType.TotalRooms;
    }
}