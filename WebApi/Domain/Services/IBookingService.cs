using Domain.Entities;

namespace Domain.Services;

public interface IBookingService
{
    Reservation Book( ReservationInput reservationInput );
}