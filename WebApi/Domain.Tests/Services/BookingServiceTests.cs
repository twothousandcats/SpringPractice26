using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Domain.Services;
using Domain.ValueObjects;
using Moq;

namespace Domain.Tests.Services;

public class BookingServiceTests
{
    [Fact]
    public void Book_HappyPath_PersistsAndReturnsReservation()
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

        RoomType roomType = new RoomType(
            Guid.NewGuid(),
            property.Id,
            "Test name",
            new Money( 100m, "Test currency" ),
            1,
            2,
            2,
            new[] { "Breakfast" },
            new[] { "Wi-Fi", "TV" }
        );

        DateRange period = new DateRange(
            new DateOnly( 2020, 01, 01 ),
            new DateOnly( 2020, 01, 03 )
        );

        ReservationInput input = new ReservationInput(
            property.Id,
            roomType.Id,
            period,
            new TimeOnly( 10, 20, 30 ),
            new TimeOnly( 10, 20, 30 ),
            2,
            "Test name",
            "88005553535"
        );

        Mock<IPropertyRepository> propertyRepositoryMock = new Mock<IPropertyRepository>();
        Mock<IRoomTypeRepository> roomTypeRepositoryMock = new Mock<IRoomTypeRepository>();
        Mock<IReservationRepository> reservationRepositoryMock = new Mock<IReservationRepository>();
        propertyRepositoryMock
            .Setup( r => r.Get( property.Id ) )
            .Returns( property );

        roomTypeRepositoryMock
            .Setup( r => r.Get( roomType.Id ) )
            .Returns( roomType );

        Mock<IAvailabilityChecker> availabilityChecker = new Mock<IAvailabilityChecker>();
        availabilityChecker
            .Setup( checker => checker.CanBook( roomType, period ) )
            .Returns( true );

        BookingService sut = new BookingService(
            propertyRepositoryMock.Object,
            roomTypeRepositoryMock.Object,
            reservationRepositoryMock.Object,
            availabilityChecker.Object
        );

        // Act
        Reservation result = sut.Book( input );

        // Assert
        Assert.Equal( property.Id, result.PropertyId );
        Assert.Equal( roomType.Id, result.RoomTypeId );
        Assert.Equal( period, result.Period );
        Assert.Equal( roomType.DailyPrice.Multiply( period.Nights ), result.Total );
        reservationRepositoryMock.Verify(
            r => r.Add( It.Is<Reservation>( x => x.Id == result.Id ) ),
            Times.Once
        );
    }

    [Fact]
    public void Book_PropertyNotFound_ThrowsEntityNotFound()
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

        RoomType roomType = new RoomType(
            Guid.NewGuid(),
            property.Id,
            "Test name",
            new Money( 100m, "Test currency" ),
            1,
            2,
            2,
            new[] { "Breakfast" },
            new[] { "Wi-Fi", "TV" }
        );

        DateRange period = new DateRange(
            new DateOnly( 2020, 01, 01 ),
            new DateOnly( 2020, 01, 03 )
        );

        ReservationInput input = new ReservationInput(
            property.Id,
            roomType.Id,
            period,
            new TimeOnly( 10, 20, 30 ),
            new TimeOnly( 10, 20, 30 ),
            2,
            "Test name",
            "88005553535"
        );

        Mock<IPropertyRepository> propertyRepositoryMock = new Mock<IPropertyRepository>();
        propertyRepositoryMock
            .Setup( r => r.Get( input.PropertyId ) )
            .Returns( ( Property? )null );

        BookingService sut = new BookingService(
            propertyRepositoryMock.Object,
            new Mock<IRoomTypeRepository>().Object,
            new Mock<IReservationRepository>().Object,
            new Mock<IAvailabilityChecker>().Object
        );

        // Act
        EntityNotFoundException ex = Assert.Throws<EntityNotFoundException>( () => sut.Book( input ) );

        // Assert
        Assert.Equal( input.PropertyId, ex.EntityId );
    }

    [Fact]
    public void Book_RoomTypeNotFound_ThrowsEntityNotFound()
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

        RoomType roomType = new RoomType(
            Guid.NewGuid(),
            property.Id,
            "Test name",
            new Money( 100m, "Test currency" ),
            1,
            2,
            2,
            new[] { "Breakfast" },
            new[] { "Wi-Fi", "TV" }
        );

        DateRange period = new DateRange(
            new DateOnly( 2020, 01, 01 ),
            new DateOnly( 2020, 01, 03 )
        );

        ReservationInput input = new ReservationInput(
            property.Id,
            roomType.Id,
            period,
            new TimeOnly( 10, 20, 30 ),
            new TimeOnly( 10, 20, 30 ),
            2,
            "Test name",
            "88005553535"
        );

        Mock<IPropertyRepository> propertyRepositoryMock = new Mock<IPropertyRepository>();
        Mock<IRoomTypeRepository> roomTypeRepositoryMock = new Mock<IRoomTypeRepository>();
        propertyRepositoryMock
            .Setup( r => r.Get( property.Id ) )
            .Returns( property );

        roomTypeRepositoryMock
            .Setup( r => r.Get( input.RoomTypeId ) )
            .Returns( ( RoomType? )null );

        BookingService sut = new BookingService(
            propertyRepositoryMock.Object,
            roomTypeRepositoryMock.Object,
            new Mock<IReservationRepository>().Object,
            new Mock<IAvailabilityChecker>().Object
        );

        // Act
        EntityNotFoundException ex = Assert.Throws<EntityNotFoundException>( () => sut.Book( input ) );

        // Assert
        Assert.Equal( input.RoomTypeId, ex.EntityId );
    }

    [Fact]
    public void Book_RoomTypeBelongsToOtherProperty_ThrowsValidation()
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

        RoomType roomType = new RoomType(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Test name",
            new Money( 100m, "Test currency" ),
            1,
            2,
            2,
            new[] { "Breakfast" },
            new[] { "Wi-Fi", "TV" }
        );

        DateRange period = new DateRange(
            new DateOnly( 2020, 01, 01 ),
            new DateOnly( 2020, 01, 03 )
        );

        ReservationInput input = new ReservationInput(
            property.Id,
            roomType.Id,
            period,
            new TimeOnly( 10, 20, 30 ),
            new TimeOnly( 10, 20, 30 ),
            2,
            "Test name",
            "88005553535"
        );

        Mock<IPropertyRepository> propertyRepositoryMock = new Mock<IPropertyRepository>();
        Mock<IRoomTypeRepository> roomTypeRepositoryMock = new Mock<IRoomTypeRepository>();
        propertyRepositoryMock
            .Setup( r => r.Get( property.Id ) )
            .Returns( property );

        roomTypeRepositoryMock
            .Setup( r => r.Get( input.RoomTypeId ) )
            .Returns( roomType );

        BookingService sut = new BookingService(
            propertyRepositoryMock.Object,
            roomTypeRepositoryMock.Object,
            new Mock<IReservationRepository>().Object,
            new Mock<IAvailabilityChecker>().Object
        );

        // Act, Assert
        Assert.Throws<BookingValidationException>( () => sut.Book( input ) );
    }

    [Fact]
    public void Book_GuestCountOutOfRange_ThrowsValidation()
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

        RoomType roomType = new RoomType(
            Guid.NewGuid(),
            property.Id,
            "Test name",
            new Money( 100m, "Test currency" ),
            1,
            2,
            2,
            new[] { "Breakfast" },
            new[] { "Wi-Fi", "TV" }
        );

        DateRange period = new DateRange(
            new DateOnly( 2020, 01, 01 ),
            new DateOnly( 2020, 01, 03 )
        );

        ReservationInput input = new ReservationInput(
            property.Id,
            roomType.Id,
            period,
            new TimeOnly( 10, 20, 30 ),
            new TimeOnly( 10, 20, 30 ),
            3, // <-
            "Test name",
            "88005553535"
        );

        Mock<IPropertyRepository> propertyRepositoryMock = new Mock<IPropertyRepository>();
        Mock<IRoomTypeRepository> roomTypeRepositoryMock = new Mock<IRoomTypeRepository>();
        propertyRepositoryMock
            .Setup( r => r.Get( property.Id ) )
            .Returns( property );

        roomTypeRepositoryMock
            .Setup( r => r.Get( input.RoomTypeId ) )
            .Returns( roomType );

        BookingService sut = new BookingService(
            propertyRepositoryMock.Object,
            roomTypeRepositoryMock.Object,
            new Mock<IReservationRepository>().Object,
            new Mock<IAvailabilityChecker>().Object
        );

        // Act, Assert
        Assert.Throws<BookingValidationException>( () => sut.Book( input ) );
    }

    [Fact]
    public void Book_NotAvailable_ThrowsValidation()
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

        RoomType roomType = new RoomType(
            Guid.NewGuid(),
            property.Id,
            "Test name",
            new Money( 100m, "Test currency" ),
            1,
            2,
            2,
            new[] { "Breakfast" },
            new[] { "Wi-Fi", "TV" }
        );

        DateRange period = new DateRange(
            new DateOnly( 2020, 01, 01 ),
            new DateOnly( 2020, 01, 03 )
        );

        ReservationInput input = new ReservationInput(
            property.Id,
            roomType.Id,
            period,
            new TimeOnly( 10, 20, 30 ),
            new TimeOnly( 10, 20, 30 ),
            2,
            "Test name",
            "88005553535"
        );

        Mock<IPropertyRepository> propertyRepositoryMock = new Mock<IPropertyRepository>();
        Mock<IRoomTypeRepository> roomTypeRepositoryMock = new Mock<IRoomTypeRepository>();
        Mock<IAvailabilityChecker> availabilityCheckerMock = new Mock<IAvailabilityChecker>();
        propertyRepositoryMock
            .Setup( r => r.Get( property.Id ) )
            .Returns( property );

        roomTypeRepositoryMock
            .Setup( r => r.Get( input.RoomTypeId ) )
            .Returns( roomType );

        availabilityCheckerMock
            .Setup( a => a.CanBook( roomType, period ) )
            .Returns( false );

        BookingService sut = new BookingService(
            propertyRepositoryMock.Object,
            roomTypeRepositoryMock.Object,
            new Mock<IReservationRepository>().Object,
            availabilityCheckerMock.Object
        );

        // Act, Assert
        Assert.Throws<BookingValidationException>( () => sut.Book( input ) );
    }

    [Fact]
    public void Book_FailureBeforeAdd_DoesNotPersist()
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

        RoomType roomType = new RoomType(
            Guid.NewGuid(),
            property.Id,
            "Test name",
            new Money( 100m, "Test currency" ),
            1,
            2,
            2,
            new[] { "Breakfast" },
            new[] { "Wi-Fi", "TV" }
        );

        DateRange period = new DateRange(
            new DateOnly( 2020, 01, 01 ),
            new DateOnly( 2020, 01, 03 )
        );

        ReservationInput input = new ReservationInput(
            property.Id,
            roomType.Id,
            period,
            new TimeOnly( 10, 20, 30 ),
            new TimeOnly( 10, 20, 30 ),
            2,
            "Test name",
            "88005553535"
        );

        Mock<IPropertyRepository> propertyRepositoryMock = new Mock<IPropertyRepository>();
        Mock<IRoomTypeRepository> roomTypeRepositoryMock = new Mock<IRoomTypeRepository>();
        Mock<IReservationRepository> reservationRepositoryMock = new Mock<IReservationRepository>();
        Mock<IAvailabilityChecker> availabilityCheckerMock = new Mock<IAvailabilityChecker>();
        propertyRepositoryMock
            .Setup( r => r.Get( property.Id ) )
            .Returns( property );

        roomTypeRepositoryMock
            .Setup( r => r.Get( input.RoomTypeId ) )
            .Returns( roomType );

        availabilityCheckerMock
            .Setup( a => a.CanBook( roomType, period ) )
            .Returns( false );

        BookingService sut = new BookingService(
            propertyRepositoryMock.Object,
            roomTypeRepositoryMock.Object,
            reservationRepositoryMock.Object,
            availabilityCheckerMock.Object
        );

        // Act, Assert
        Assert.Throws<BookingValidationException>( () => sut.Book( input ) );
        reservationRepositoryMock.Verify( r => r.Add( It.IsAny<Reservation>() ), Times.Never );
    }
}