using Domain.Entities;
using Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts.Properties;

namespace WebApi.Controllers;

[ApiController]
public sealed class PropertiesController : ControllerBase
{
    private readonly IPropertyRepository _propertyRepository;

    public PropertiesController( IPropertyRepository propertyRepository )
    {
        _propertyRepository = propertyRepository;
    }

    [HttpGet]
    public ActionResult<IReadOnlyCollection<PropertyDto>> GetAll()
    {
        PropertyDto[] result = _propertyRepository.GetAll().Select( ToDto ).ToArray();

        return Ok( result );
    }

    [HttpGet( "{id:guid}", Name = "GetProperty" )]
    public ActionResult<PropertyDto> Get( Guid id )
    {
        Property? property = _propertyRepository.Get( id );

        return property is null
            ? NotFound()
            : Ok( ToDto( property ) );
    }

    [HttpPost]
    public ActionResult<PropertyDto> Create( [FromBody] CreatePropertyRequest request )
    {
        Property property = new Property(
            Guid.NewGuid(),
            request.Name,
            request.Country,
            request.City,
            request.Address,
            request.Latitude,
            request.Longitude
        );

        _propertyRepository.Add( property );

        return CreatedAtRoute(
            "GetProperty",
            new
            {
                id = property.Id
            },
            ToDto( property )
        );
    }

    [HttpPut( "{id:guid}" )]
    public IActionResult Update(
        Guid id,
        [FromBody] UpdatePropertyRequest request
    )
    {
        Property? property = _propertyRepository.Get( id );
        if ( property is null )
        {
            return NotFound();
        }

        property.Update(
            request.Name,
            request.Country,
            request.City,
            request.Address,
            request.Latitude,
            request.Longitude
        );

        _propertyRepository.Update( property );

        return NoContent();
    }

    [HttpDelete( "{id:guid}" )]
    public IActionResult Remove( Guid id )
    {
        return _propertyRepository.Remove( id )
            ? NoContent()
            : NotFound();
    }

    private static PropertyDto ToDto( Property property )
    {
        return new PropertyDto(
            property.Id,
            property.Name,
            property.Country,
            property.City,
            property.Address,
            property.Latitude,
            property.Longitude
        );
    }
}