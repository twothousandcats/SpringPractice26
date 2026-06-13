using Domain.Entities;
using WebApi.Contracts.Common;

namespace WebApi.Contracts.Reservations;

public static class ReservationMappings
{
    public static ReservationDto ToDto( this Reservation reservation )
    {
        return new ReservationDto(
            reservation.Id,
            reservation.PropertyId,
            reservation.RoomTypeId,
            reservation.Period.Start,
            reservation.Period.End,
            reservation.ArrivalTime,
            reservation.DepartureTime,
            reservation.GuestName,
            reservation.GuestPhoneNumber,
            reservation.Total.ToDto(),
            reservation.Status.ToString()
        );
    }
}