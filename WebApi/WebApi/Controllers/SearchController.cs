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
            .Select( variant => variant.ToDto() )
            .ToArray();

        return Ok( result );
    }
}