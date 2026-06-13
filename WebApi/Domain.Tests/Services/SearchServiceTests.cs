using Domain.Entities;
using Domain.Repositories;
using Domain.Services;
using Domain.ValueObjects;
using Moq;

namespace Domain.Tests.Services;

public class SearchServiceTests
{
    [Fact]
    public void Search_NoPropertiesInCity_ReturnsEmpty()
    {
        // Arrange
        DateRange period = new DateRange(
            new DateOnly( 2020, 01, 01 ),
            new DateOnly( 2020, 01, 03 )
        );

        Mock<IPropertyRepository> propertyRepositoryMock = new Mock<IPropertyRepository>();
        propertyRepositoryMock
            .Setup( r => r.GetByCity( "Test city" ) )
            .Returns( Array.Empty<Property>() );

        SearchCriteria criteria = new SearchCriteria(
            "Test city",
            period,
            2,
            100m
        );

        SearchService sut = new SearchService(
            propertyRepositoryMock.Object,
            new Mock<IRoomTypeRepository>().Object,
            new Mock<IAvailabilityChecker>().Object
        );

        // Act
        IReadOnlyCollection<SearchVariant> result = sut.Search( criteria );

        // Assert
        Assert.Empty( result );
    }

    [Fact]
    public void Search_MatchingRoomType_ReturnsVariantWithComputedTotal()
    {
        // Arrange
        Property property = new Property(
            Guid.NewGuid(),
            "Test name",
            "Test country",
            "Test city",
            "Test address",
            10.10,
            20.20
        );

        DateRange period = new DateRange(
            new DateOnly( 2020, 01, 01 ),
            new DateOnly( 2020, 01, 03 )
        );

        RoomType roomType = new RoomType(
            Guid.NewGuid(),
            property.Id,
            "Standard",
            new Money( 100m, "Test currency" ),
            1,
            2,
            2,
            Array.Empty<string>(),
            Array.Empty<string>()
        );

        Mock<IPropertyRepository> propertyRepositoryMock = new Mock<IPropertyRepository>();
        propertyRepositoryMock
            .Setup( r => r.GetByCity( "Test city" ) )
            .Returns( new[] { property } );

        Mock<IRoomTypeRepository> roomTypeRepositoryMock = new Mock<IRoomTypeRepository>();
        roomTypeRepositoryMock
            .Setup( r => r.GetByProperty( property.Id ) )
            .Returns( new[] { roomType } );

        Mock<IAvailabilityChecker> availabilityCheckerMock = new Mock<IAvailabilityChecker>();
        availabilityCheckerMock
            .Setup( a => a.CanBook( It.IsAny<RoomType>(), It.IsAny<DateRange>() ) )
            .Returns( true );

        SearchCriteria criteria = new SearchCriteria(
            "Test city",
            period,
            2,
            100m
        );

        SearchService sut = new SearchService(
            propertyRepositoryMock.Object,
            roomTypeRepositoryMock.Object,
            availabilityCheckerMock.Object
        );

        // Act
        IReadOnlyCollection<SearchVariant> result = sut.Search( criteria );

        // Assert
        SearchVariant variant = Assert.Single( result );
        Assert.Equal( property, variant.Property );
        Assert.Equal( roomType, variant.RoomType );
        Assert.Equal( new Money( 200m, "Test currency" ), variant.TotalForPeriod );
    }

    [Fact]
    public void Search_GuestCountOutOfRange_RoomTypeFilteredOut()
    {
        // Arrange
        Property property = new Property(
            Guid.NewGuid(),
            "Test city",
            "Test country",
            "Test city",
            "Test address",
            10.10,
            20.20
        );

        DateRange period = new DateRange(
            new DateOnly( 2020, 01, 01 ),
            new DateOnly( 2020, 01, 03 )
        );

        RoomType roomType = new RoomType(
            Guid.NewGuid(),
            property.Id,
            "Standard",
            new Money( 100m, "Test currency" ),
            1,
            2,
            2,
            Array.Empty<string>(),
            Array.Empty<string>()
        );

        Mock<IPropertyRepository> propertyRepositoryMock = new Mock<IPropertyRepository>();
        propertyRepositoryMock
            .Setup( r => r.GetByCity( "Test city" ) )
            .Returns( new[] { property } );

        Mock<IRoomTypeRepository> roomTypeRepositoryMock = new Mock<IRoomTypeRepository>();
        roomTypeRepositoryMock
            .Setup( r => r.GetByProperty( property.Id ) )
            .Returns( new[] { roomType } );

        SearchCriteria criteria = new SearchCriteria(
            "Test city",
            period,
            5, // <-
            100m
        );

        SearchService sut = new SearchService(
            propertyRepositoryMock.Object,
            roomTypeRepositoryMock.Object,
            new Mock<IAvailabilityChecker>().Object
        );

        // Act
        IReadOnlyCollection<SearchVariant> result = sut.Search( criteria );

        // Assert
        Assert.Empty( result );
    }

    [Fact]
    public void Search_AbovePriceCap_RoomTypeFilteredOut()
    {
        // Arrange
        Property property = new Property(
            Guid.NewGuid(),
            "Test city",
            "Test country",
            "Test city",
            "Test address",
            10.10,
            20.20
        );

        DateRange period = new DateRange(
            new DateOnly( 2020, 01, 01 ),
            new DateOnly( 2020, 01, 03 )
        );

        RoomType roomType = new RoomType(
            Guid.NewGuid(),
            property.Id,
            "Standard",
            new Money( 500m, "Test currency" ),
            1,
            2,
            2,
            Array.Empty<string>(),
            Array.Empty<string>()
        );

        Mock<IPropertyRepository> propertyRepositoryMock = new Mock<IPropertyRepository>();
        propertyRepositoryMock
            .Setup( r => r.GetByCity( "Test city" ) )
            .Returns( new[] { property } );

        Mock<IRoomTypeRepository> roomTypeRepositoryMock = new Mock<IRoomTypeRepository>();
        roomTypeRepositoryMock
            .Setup( r => r.GetByProperty( property.Id ) )
            .Returns( new[] { roomType } );

        SearchCriteria criteria = new SearchCriteria(
            "Test city",
            period,
            2,
            200m
        );

        SearchService sut = new SearchService(
            propertyRepositoryMock.Object,
            roomTypeRepositoryMock.Object,
            new Mock<IAvailabilityChecker>().Object
        );

        // Act
        IReadOnlyCollection<SearchVariant> result = sut.Search( criteria );

        // Assert
        Assert.Empty( result );
    }

    [Fact]
    public void Search_NotAvailable_RoomTypeFilteredOut()
    {
        // Arrange
        Property property = new Property(
            Guid.NewGuid(),
            "Test city",
            "Test country",
            "Test city",
            "Test address",
            10.10,
            20.20
        );

        DateRange period = new DateRange(
            new DateOnly( 2020, 01, 01 ),
            new DateOnly( 2020, 01, 03 )
        );

        RoomType roomType = new RoomType(
            Guid.NewGuid(),
            property.Id,
            "Standard",
            new Money( 100m, "Test currency" ),
            1,
            2,
            2,
            Array.Empty<string>(),
            Array.Empty<string>()
        );

        Mock<IPropertyRepository> propertyRepositoryMock = new Mock<IPropertyRepository>();
        propertyRepositoryMock
            .Setup( r => r.GetByCity( "Test city" ) )
            .Returns( new[] { property } );

        Mock<IRoomTypeRepository> roomTypeRepositoryMock = new Mock<IRoomTypeRepository>();
        roomTypeRepositoryMock
            .Setup( r => r.GetByProperty( property.Id ) )
            .Returns( new[] { roomType } );

        Mock<IAvailabilityChecker> availabilityCheckerMock = new Mock<IAvailabilityChecker>();
        availabilityCheckerMock
            .Setup( a => a.CanBook( roomType, period ) )
            .Returns( false );

        SearchCriteria criteria = new SearchCriteria(
            "Test city",
            period,
            2
        );

        SearchService sut = new SearchService(
            propertyRepositoryMock.Object,
            roomTypeRepositoryMock.Object,
            availabilityCheckerMock.Object
        );

        // Act
        IReadOnlyCollection<SearchVariant> result = sut.Search( criteria );

        // Assert
        Assert.Empty( result );
    }

    [Fact]
    public void Search_MultipleProperties_AggregatesVariants()
    {
        // Arrange
        Property firstProperty = new Property(
            Guid.NewGuid(),
            "Test city",
            "Test country",
            "Test city",
            "Test address",
            10.10,
            20.20
        );

        Property secondProperty = new Property(
            Guid.NewGuid(),
            "Test city2",
            "Test country2",
            "Test city2",
            "Test address2",
            20.20,
            30.30
        );

        DateRange period = new DateRange(
            new DateOnly( 2020, 01, 01 ),
            new DateOnly( 2020, 01, 03 )
        );

        RoomType firstRoomType = new RoomType(
            Guid.NewGuid(),
            firstProperty.Id,
            "Standard",
            new Money( 100m, "Test currency" ),
            1,
            2,
            2,
            Array.Empty<string>(),
            Array.Empty<string>()
        );

        RoomType secondRoomType = new RoomType(
            Guid.NewGuid(),
            secondProperty.Id,
            "Standard",
            new Money( 150m, "Test currency" ),
            1,
            2,
            2,
            Array.Empty<string>(),
            Array.Empty<string>()
        );

        Mock<IPropertyRepository> propertyRepositoryMock = new Mock<IPropertyRepository>();
        propertyRepositoryMock
            .Setup( r => r.GetByCity( "Test city" ) )
            .Returns( new[] { firstProperty, secondProperty } );

        Mock<IRoomTypeRepository> roomTypeRepositoryMock = new Mock<IRoomTypeRepository>();
        roomTypeRepositoryMock
            .Setup( r => r.GetByProperty( firstProperty.Id ) )
            .Returns( new[] { firstRoomType } );

        roomTypeRepositoryMock
            .Setup( r => r.GetByProperty( secondProperty.Id ) )
            .Returns( new[] { secondRoomType } );

        Mock<IAvailabilityChecker> availabilityCheckerMock = new Mock<IAvailabilityChecker>();
        availabilityCheckerMock
            .Setup( a => a.CanBook( It.IsAny<RoomType>(), It.IsAny<DateRange>() ) )
            .Returns( true );

        SearchCriteria criteria = new SearchCriteria(
            "Test city",
            period,
            2
        );

        SearchService sut = new SearchService(
            propertyRepositoryMock.Object,
            roomTypeRepositoryMock.Object,
            availabilityCheckerMock.Object
        );

        // Act
        IReadOnlyCollection<SearchVariant> result = sut.Search( criteria );

        // Assert
        Assert.Equal( 2, result.Count );
    }
}