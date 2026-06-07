using Domain.Entities;
using Domain.Repositories;
using Domain.Services;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts.Common;
using WebApi.Contracts.Reservations;

namespace WebApi.Controllers;

[ApiController]
[Route( "api/reservations" )]
public sealed class ReservationsController : ControllerBase
{
    private readonly IReservationRepository _reservationRepository;

    private readonly IBookingService _bookingService;

    public ReservationsController(
        IReservationRepository reservationRepository,
        IBookingService bookingService
    )
    {
        _reservationRepository = reservationRepository;
        _bookingService = bookingService;
    }

    [HttpGet]
    public ActionResult<IReadOnlyCollection<ReservationDto>> Search(
        [FromQuery] Guid? propertyId,
        [FromQuery] Guid? roomTypeId,
        [FromQuery] string? guestName
    )
    {
        IEnumerable<Reservation> reservations = _reservationRepository.Search();

        if ( propertyId is not null )
        {
            reservations = reservations.Where( r => r.PropertyId == propertyId.Value );
        }

        if ( roomTypeId is not null )
        {
            reservations = reservations.Where( r => r.RoomTypeId == roomTypeId.Value );
        }

        if ( !string.IsNullOrWhiteSpace( guestName ) )
        {
            reservations = reservations
                .Where( r => r.GuestName.Contains( guestName, StringComparison.OrdinalIgnoreCase ) );
        }

        return Ok(
            reservations
                .Select( ToDto )
                .ToArray()
        );
    }

    [HttpGet( "{id:guid}", Name = "GetReservation" )]
    public ActionResult<ReservationDto> Get( Guid id )
    {
        Reservation? reservation = _reservationRepository.Get( id );

        return reservation is null
            ? NotFound()
            : Ok( ToDto( reservation ) );
    }

    [HttpPost]
    public ActionResult<ReservationDto> Create( [FromBody] CreateReservationRequest request )
    {
        ReservationInput input = new ReservationInput(
            request.PropertyId,
            request.RoomTypeId,
            new DateRange( request.ArrivalDate, request.DepartureDate ),
            request.ArrivalTime,
            request.DepartureTime,
            request.GuestCount,
            request.GuestName,
            request.GuestPhoneNumber
        );

        Reservation reservation = _bookingService.Book( input );

        return CreatedAtRoute(
            "GetReservation", new
            {
                id = reservation.Id
            }, ToDto( reservation )
        );
    }

    [HttpDelete( "{id:guid}" )]
    public IActionResult Cancel( Guid id )
    {
        Reservation? reservation = _reservationRepository.Get( id );
        if ( reservation is null )
        {
            return NotFound();
        }

        reservation.Cancel();
        _reservationRepository.Update( reservation );

        return NoContent();
    }

    private static ReservationDto ToDto( Reservation reservation )
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
            new MoneyDto( reservation.Total.Amount, reservation.Total.Currency ),
            reservation.Status.ToString()
        );
    }
}