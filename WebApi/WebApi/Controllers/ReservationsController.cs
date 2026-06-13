using Domain.Entities;
using Domain.Repositories;
using Domain.Services;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;
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
        [FromQuery] string? guestName,
        [FromQuery] DateOnly? arrivalDate,
        [FromQuery] DateOnly? departureDate
    )
    {
        if ( arrivalDate.HasValue != departureDate.HasValue )
        {
            return BadRequest( "Arrival date and departure dates must be provided together" );
        }

        DateRange? period = arrivalDate.HasValue
            ? new DateRange( arrivalDate.Value, departureDate!.Value )
            : null;

        ReservationFilter reservationFilter = new ReservationFilter(
            propertyId,
            roomTypeId,
            guestName,
            period
        );

        ReservationDto[] reservationDtos = _reservationRepository.Search( reservationFilter )
            .Select( reservation => reservation.ToDto() )
            .ToArray();

        return Ok( reservationDtos );
    }

    [HttpGet( "{id:guid}", Name = "GetReservation" )]
    public ActionResult<ReservationDto> Get( Guid id )
    {
        Reservation? reservation = _reservationRepository.Get( id );

        return reservation is null
            ? NotFound()
            : Ok( reservation.ToDto() );
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
            },
            reservation.ToDto()
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
}