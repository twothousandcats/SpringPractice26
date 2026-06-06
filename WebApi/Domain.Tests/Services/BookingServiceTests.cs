using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Domain.Services;
using Domain.ValueObjects;
using Moq;

namespace Domain.Tests.Services;

public class BookingServiceTests
{
    private const string HotelName = "Azimut";

    private const string HotelCountry = "Russia";

    private const string HotelCity = "Yoshkar-Ola";

    private const string HotelAddress = "Voskresensky Prospect, Building 11";

    private const double HotelLatitude = 56.63;

    private const double HotelLongitude = 47.91;

    private const string Currency = "EUR";

    private const string GuestName = "Ivan Ivanov";

    private const string GuestPhone = "+79991234567";

    private static readonly DateRange Period = new DateRange(
        new DateOnly( 2026, 6, 1 ),
        new DateOnly( 2026, 6, 4 )
    );

    private static readonly TimeOnly ArrivalTime = new TimeOnly( 14, 0 );

    private static readonly TimeOnly DepartureTime = new TimeOnly( 12, 0 );

    private readonly Mock<IPropertyRepository> _propertyRepositoryMock = new Mock<IPropertyRepository>();

    private readonly Mock<IRoomTypeRepository> _roomTypeRepositoryMock = new Mock<IRoomTypeRepository>();

    private readonly Mock<IReservationRepository> _reservationRepositoryMock = new Mock<IReservationRepository>();

    private readonly Mock<IAvailabilityChecker> _availabilityCheckerMock = new Mock<IAvailabilityChecker>();

    private readonly BookingService _bookingService;

    private void SetupRepositoriesWith( Property property, RoomType roomType )
    {
        _propertyRepositoryMock
            .Setup( r => r.Get( property.Id ) )
            .Returns( property );

        _roomTypeRepositoryMock
            .Setup( r => r.Get( roomType.Id ) )
            .Returns( roomType );
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

    private static RoomType CreateRoomType( Guid propertyId )
    {
        return new RoomType(
            Guid.NewGuid(),
            propertyId,
            "Standard",
            new Money( 100m, Currency ),
            1,
            2,
            2,
            Array.Empty<string>(),
            Array.Empty<string>()
        );
    }

    private static ReservationInput CreateInput( Guid propertyId, Guid roomTypeId, int guestCount )
    {
        return new ReservationInput(
            propertyId,
            roomTypeId,
            Period,
            ArrivalTime,
            DepartureTime,
            guestCount,
            GuestName,
            GuestPhone
        );
    }

    public BookingServiceTests()
    {
        _bookingService = new BookingService(
            _propertyRepositoryMock.Object,
            _roomTypeRepositoryMock.Object,
            _reservationRepositoryMock.Object,
            _availabilityCheckerMock.Object
        );
    }

    [Fact]
    public void Book_HappyPath_PersistsAndReturnsReservation()
    {
        Property property = CreateProperty();
        RoomType roomType = CreateRoomType( property.Id );
        ReservationInput input = CreateInput( property.Id, roomType.Id, guestCount: 2 );
        SetupRepositoriesWith( property, roomType );
        _availabilityCheckerMock
            .Setup( a => a.CanBook( roomType, Period ) )
            .Returns( true );

        Reservation result = _bookingService.Book( input );

        Assert.Equal( property.Id, result.PropertyId );
        Assert.Equal( roomType.Id, result.RoomTypeId );
        Assert.Equal( Period, result.Period );
        Assert.Equal( roomType.DailyPrice.Multiply( Period.Nights ), result.Total );
        _reservationRepositoryMock.Verify(
            r => r.Add( It.Is<Reservation>( x => x.Id == result.Id ) ),
            Times.Once
        );
    }

    [Fact]
    public void Book_PropertyNotFound_ThrowsEntityNotFound()
    {
        ReservationInput input = CreateInput( Guid.NewGuid(), Guid.NewGuid(), guestCount: 2 );
        _propertyRepositoryMock
            .Setup( r => r.Get( input.PropertyId ) )
            .Returns( ( Property? )null );

        EntityNotFoundException ex = Assert.Throws<EntityNotFoundException>( () => _bookingService.Book( input ) );
        Assert.Equal( input.PropertyId, ex.EntityId );
    }

    [Fact]
    public void Book_RoomTypeNotFound_ThrowsEntityNotFound()
    {
        Property property = CreateProperty();
        ReservationInput input = CreateInput( property.Id, Guid.NewGuid(), guestCount: 2 );
        _propertyRepositoryMock
            .Setup( r => r.Get( property.Id ) )
            .Returns( property );

        _roomTypeRepositoryMock
            .Setup( r => r.Get( input.RoomTypeId ) )
            .Returns( ( RoomType? )null );

        EntityNotFoundException ex = Assert.Throws<EntityNotFoundException>( () => _bookingService.Book( input ) );
        Assert.Equal( input.RoomTypeId, ex.EntityId );
    }

    [Fact]
    public void Book_RoomTypeBelongsToOtherProperty_ThrowsValidation()
    {
        Property property = CreateProperty();
        RoomType roomType = CreateRoomType( propertyId: Guid.NewGuid() );
        ReservationInput input = CreateInput( property.Id, roomType.Id, guestCount: 2 );
        SetupRepositoriesWith( property, roomType );

        Assert.Throws<BookingValidationException>( () => _bookingService.Book( input ) );
    }

    [Fact]
    public void Book_GuestCountOutOfRange_ThrowsValidation()
    {
        Property property = CreateProperty();
        RoomType roomType = CreateRoomType( property.Id );
        ReservationInput input = CreateInput( property.Id, roomType.Id, guestCount: 10 );
        SetupRepositoriesWith( property, roomType );

        Assert.Throws<BookingValidationException>( () => _bookingService.Book( input ) );
    }

    [Fact]
    public void Book_NotAvailable_ThrowsValidation()
    {
        Property property = CreateProperty();
        RoomType roomType = CreateRoomType( property.Id );
        ReservationInput input = CreateInput( property.Id, roomType.Id, guestCount: 2 );
        SetupRepositoriesWith( property, roomType );
        _availabilityCheckerMock
            .Setup( a => a.CanBook( roomType, Period ) )
            .Returns( false );

        Assert.Throws<BookingValidationException>( () => _bookingService.Book( input ) );
    }

    [Fact]
    public void Book_FailureBeforeAdd_DoesNotPersist()
    {
        Property property = CreateProperty();
        RoomType roomType = CreateRoomType( property.Id );
        ReservationInput input = CreateInput( property.Id, roomType.Id, guestCount: 2 );
        SetupRepositoriesWith( property, roomType );
        _availabilityCheckerMock
            .Setup( a => a.CanBook( roomType, Period ) )
            .Returns( false );

        Assert.Throws<BookingValidationException>( () => _bookingService.Book( input ) );
        _reservationRepositoryMock.Verify( r => r.Add( It.IsAny<Reservation>() ), Times.Never );
    }
}