using Domain.Entities;
using Domain.Repositories;
using Domain.Services;
using Domain.ValueObjects;
using Moq;

namespace Domain.Tests.Services;

public class SearchServiceTests
{
    private const string HotelName = "Azimut";

    private const string HotelCountry = "Russia";

    private const string HotelCity = "Yoshkar-Ola";

    private const string HotelAddress = "Voskresensky Prospect, Building 11";

    private const double HotelLatitude = 56.63;

    private const double HotelLongitude = 47.91;

    private const string Currency = "EUR";

    private static readonly DateRange Period = new DateRange(
        new DateOnly( 2026, 6, 1 ),
        new DateOnly( 2026, 6, 4 )
    );

    private readonly Mock<IPropertyRepository> _propertyRepositoryMock = new Mock<IPropertyRepository>();

    private readonly Mock<IRoomTypeRepository> _roomTypeRepositoryMock = new Mock<IRoomTypeRepository>();

    private readonly Mock<IAvailabilityChecker> _availabilityCheckerMock = new Mock<IAvailabilityChecker>();

    private readonly SearchService _searchService;

    private void SetupCityAndRooms( Property property, RoomType roomType )
    {
        _propertyRepositoryMock
            .Setup( r => r.GetByCity( HotelCity ) )
            .Returns( new[] { property } );

        _roomTypeRepositoryMock
            .Setup( r => r.GetByProperty( property.Id ) )
            .Returns( new[] { roomType } );
    }

    private static SearchCriteria CreateCriteria( int guestCount, decimal? maxPricePerNight = null )
    {
        return new SearchCriteria( HotelCity, Period, guestCount, maxPricePerNight );
    }

    private static Property CreateProperty()
    {
        return new Property(
            Guid.NewGuid(),
            HotelName,
            HotelCountry,
            HotelCity,
            HotelAddress,
            HotelLatitude,
            HotelLongitude
        );
    }

    private static RoomType CreateRoomType( Guid propertyId, decimal dailyPrice, int min, int max )
    {
        return new RoomType(
            Guid.NewGuid(),
            propertyId,
            "Standard",
            new Money( dailyPrice, Currency ),
            min,
            max,
            2,
            Array.Empty<string>(),
            Array.Empty<string>()
        );
    }

    public SearchServiceTests()
    {
        _searchService = new SearchService(
            _propertyRepositoryMock.Object,
            _roomTypeRepositoryMock.Object,
            _availabilityCheckerMock.Object
        );

        _availabilityCheckerMock
            .Setup( a => a.CanBook( It.IsAny<RoomType>(), It.IsAny<DateRange>() ) )
            .Returns( true );
    }

    [Fact]
    public void Search_NoPropertiesInCity_ReturnsEmpty()
    {
        SearchCriteria criteria = CreateCriteria( guestCount: 2 );
        _propertyRepositoryMock.Setup( r => r.GetByCity( HotelCity ) ).Returns( Array.Empty<Property>() );

        IReadOnlyCollection<SearchVariant> result = _searchService.Search( criteria );

        Assert.Empty( result );
    }

    [Fact]
    public void Search_MatchingRoomType_ReturnsVariantWithComputedTotal()
    {
        Property property = CreateProperty();
        RoomType roomType = CreateRoomType( property.Id, dailyPrice: 100m, min: 1, max: 2 );
        SetupCityAndRooms( property, roomType );
        SearchCriteria criteria = CreateCriteria( guestCount: 2 );

        IReadOnlyCollection<SearchVariant> result = _searchService.Search( criteria );

        SearchVariant variant = Assert.Single( result );
        Assert.Equal( property, variant.Property );
        Assert.Equal( roomType, variant.RoomType );
        Assert.Equal( new Money( 300m, Currency ), variant.TotalForPeriod );
    }

    [Fact]
    public void Search_GuestCountOutOfRange_RoomTypeFilteredOut()
    {
        Property property = CreateProperty();
        RoomType roomType = CreateRoomType( property.Id, dailyPrice: 100m, min: 1, max: 2 );
        SetupCityAndRooms( property, roomType );
        SearchCriteria criteria = CreateCriteria( guestCount: 5 );

        IReadOnlyCollection<SearchVariant> result = _searchService.Search( criteria );

        Assert.Empty( result );
    }

    [Fact]
    public void Search_AbovePriceCap_RoomTypeFilteredOut()
    {
        Property property = CreateProperty();
        RoomType roomType = CreateRoomType( property.Id, dailyPrice: 500m, min: 1, max: 2 );
        SetupCityAndRooms( property, roomType );
        SearchCriteria criteria = CreateCriteria( guestCount: 2, maxPricePerNight: 200m );

        IReadOnlyCollection<SearchVariant> result = _searchService.Search( criteria );

        Assert.Empty( result );
    }

    [Fact]
    public void Search_NotAvailable_RoomTypeFilteredOut()
    {
        Property property = CreateProperty();
        RoomType roomType = CreateRoomType( property.Id, dailyPrice: 100m, min: 1, max: 2 );
        SetupCityAndRooms( property, roomType );
        _availabilityCheckerMock
            .Setup( a => a.CanBook( roomType, Period ) )
            .Returns( false );

        SearchCriteria criteria = CreateCriteria( guestCount: 2 );

        IReadOnlyCollection<SearchVariant> result = _searchService.Search( criteria );

        Assert.Empty( result );
    }

    [Fact]
    public void Search_MultipleProperties_AggregatesVariants()
    {
        Property first = CreateProperty();
        Property second = CreateProperty();
        RoomType firstRoom = CreateRoomType( first.Id, dailyPrice: 100m, min: 1, max: 2 );
        RoomType secondRoom = CreateRoomType( second.Id, dailyPrice: 150m, min: 1, max: 2 );
        _propertyRepositoryMock.Setup( r => r.GetByCity( HotelCity ) )
            .Returns( new[] { first, second } );

        _roomTypeRepositoryMock.Setup( r => r.GetByProperty( first.Id ) )
            .Returns( new[] { firstRoom } );

        _roomTypeRepositoryMock.Setup( r => r.GetByProperty( second.Id ) )
            .Returns( new[] { secondRoom } );

        SearchCriteria criteria = CreateCriteria( guestCount: 2 );

        IReadOnlyCollection<SearchVariant> result = _searchService.Search( criteria );

        Assert.Equal( 2, result.Count );
    }
}