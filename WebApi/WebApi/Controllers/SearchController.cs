using Domain.Services;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts.Common;
using WebApi.Contracts.Properties;
using WebApi.Contracts.RoomTypes;
using WebApi.Contracts.Search;

namespace WebApi.Controllers;

[ApiController]
[Route( "api/search" )]
public sealed class SearchController : ControllerBase
{
    private readonly ISearchService _searchService;

    public SearchController( ISearchService searchService )
    {
        _searchService = searchService;
    }

    [HttpGet]
    public ActionResult<IReadOnlyCollection<SearchVariantDto>> Search(
        [FromQuery] string city,
        [FromQuery] DateOnly arrivalDate,
        [FromQuery] DateOnly departureDate,
        [FromQuery] int guests,
        [FromQuery] decimal? maxPrice
    )
    {
        SearchCriteria criteria = new SearchCriteria(
            city,
            new DateRange( arrivalDate, departureDate ),
            guests,
            maxPrice
        );

        SearchVariantDto[] result = _searchService
            .Search( criteria )
            .Select( ToDto )
            .ToArray();

        return Ok( result );
    }

    private static SearchVariantDto ToDto( SearchVariant variant )
    {
        PropertyDto property = new PropertyDto(
            variant.Property.Id,
            variant.Property.Name,
            variant.Property.Country,
            variant.Property.City,
            variant.Property.Address,
            variant.Property.Latitude,
            variant.Property.Longitude
        );

        RoomTypeDto roomType = new RoomTypeDto(
            variant.RoomType.Id,
            variant.RoomType.PropertyId,
            variant.RoomType.Name,
            new MoneyDto( variant.RoomType.DailyPrice.Amount, variant.RoomType.DailyPrice.Currency ),
            variant.RoomType.MinPersonCount,
            variant.RoomType.MaxPersonCount,
            variant.RoomType.TotalRooms,
            variant.RoomType.Services,
            variant.RoomType.Amenities
        );

        return new SearchVariantDto(
            property,
            roomType,
            new MoneyDto( variant.TotalForPeriod.Amount, variant.TotalForPeriod.Currency )
        );
    }
}