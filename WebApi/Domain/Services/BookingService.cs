using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;

namespace Domain.Services;

public class BookingService : IBookingService
{
    private readonly IPropertyRepository _propertyRepository;

    private readonly IRoomTypeRepository _roomTypeRepository;

    private readonly IReservationRepository _reservationRepository;

    private readonly IAvailabilityChecker _availabilityChecker;

    public BookingService(
        IPropertyRepository propertyRepository,
        IRoomTypeRepository roomTypeRepository,
        IReservationRepository reservationRepository,
        IAvailabilityChecker availabilityChecker
    )
    {
        _propertyRepository = propertyRepository;
        _roomTypeRepository = roomTypeRepository;
        _reservationRepository = reservationRepository;
        _availabilityChecker = availabilityChecker;
    }

    public Reservation Book( ReservationInput reservationInput )
    {
        Property property = _propertyRepository.Get( reservationInput.PropertyId ) ??
                            throw new EntityNotFoundException( nameof( Property ), reservationInput.PropertyId );

        RoomType roomType = _roomTypeRepository.Get( reservationInput.RoomTypeId ) ??
                            throw new EntityNotFoundException( nameof( RoomType ), reservationInput.RoomTypeId );

        if ( roomType.PropertyId != property.Id )
        {
            throw new BookingValidationException( "RoomType does not belong to the specified Property." );
        }

        if ( roomType.IsFits( reservationInput.GuestCount ) )
        {
            throw new BookingValidationException( "Guest count does not match the RoomType limits." );
        }

        if ( !_availabilityChecker.CanBook( roomType, reservationInput.Period ) )
        {
            throw new BookingValidationException( "Selected RoomType is not available for the requested period." );
        }

        Reservation reservation = new Reservation(
            Guid.NewGuid(),
            property.Id,
            roomType.Id,
            reservationInput.Period,
            reservationInput.ArrivalDate,
            reservationInput.DepartureDate,
            reservationInput.GuestName,
            reservationInput.GuestPhoneNumber,
            roomType.DailyPrice
        );

        _reservationRepository.Add( reservation );
        return reservation;
    }
}