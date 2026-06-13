using Domain.Entities;
using Domain.Repositories;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts.RoomTypes;

namespace WebApi.Controllers;

[ApiController]
public sealed class RoomTypesController : ControllerBase
{
    private readonly IRoomTypeRepository _roomTypeRepository;

    private readonly IPropertyRepository _propertyRepository;

    public RoomTypesController(
        IRoomTypeRepository roomTypeRepository,
        IPropertyRepository propertyRepository
    )
    {
        _roomTypeRepository = roomTypeRepository;
        _propertyRepository = propertyRepository;
    }

    [HttpGet( "api/properties/{propertyId:guid}/roomtypes" )]
    public ActionResult<IReadOnlyCollection<RoomTypeDto>> GetByProperty( Guid propertyId )
    {
        if ( _propertyRepository.Get( propertyId ) is null )
        {
            return NotFound();
        }

        RoomTypeDto[] result = _roomTypeRepository.GetByProperty( propertyId )
            .Select( roomType => roomType.ToDto() )
            .ToArray();

        return Ok( result );
    }

    [HttpGet( "api/roomtypes/{id:guid}", Name = "GetRoomType" )]
    public ActionResult<RoomTypeDto> Get( Guid id )
    {
        RoomType? roomType = _roomTypeRepository.Get( id );
        return roomType is null
            ? NotFound()
            : Ok( roomType.ToDto() );
    }

    [HttpPost( "api/properties/{propertyId:guid}/roomtypes" )]
    public ActionResult<RoomTypeDto> Create(
        Guid propertyId,
        [FromBody] CreateRoomTypeRequest request
    )
    {
        if ( _propertyRepository.Get( propertyId ) is null )
        {
            return NotFound();
        }

        RoomType roomType = new RoomType(
            Guid.NewGuid(),
            propertyId,
            request.Name,
            new Money( request.DailyPrice.Amount, request.DailyPrice.Currency ),
            request.MinPersonCount,
            request.MaxPersonCount,
            request.TotalRooms,
            request.Services,
            request.Amenities
        );

        _roomTypeRepository.Add( roomType );

        return CreatedAtRoute(
            "GetRoomType",
            new
            {
                id = roomType.Id
            },
            roomType.ToDto()
        );
    }

    [HttpPut( "api/roomtypes/{id:guid}" )]
    public IActionResult Update(
        Guid id,
        [FromBody] UpdateRoomTypeRequest request
    )
    {
        RoomType? roomType = _roomTypeRepository.Get( id );
        if ( roomType is null )
        {
            return NotFound();
        }

        roomType.Update(
            request.Name,
            new Money( request.DailyPrice.Amount, request.DailyPrice.Currency ),
            request.MinPersonCount,
            request.MaxPersonCount,
            request.TotalRooms,
            request.Services,
            request.Amenities
        );

        _roomTypeRepository.Update( roomType );

        return NoContent();
    }

    [HttpDelete( "api/roomtypes/{id:guid}" )]
    public IActionResult Remove( Guid id )
    {
        return _roomTypeRepository.Remove( id )
            ? NoContent()
            : NotFound();
    }
}