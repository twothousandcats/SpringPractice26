using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Domain.ValueObjects;
using Infrastructure.Repositories;

namespace Infrastructure.Tests.Repositories;

public class InMemoryReservationRepositoryTests
{
    private const string Currency = "EUR";

    private static readonly Money DefaultDailyPrice = new Money( 100m, Currency );

    private readonly InMemoryReservationRepository _repository = new InMemoryReservationRepository();

    private static Reservation CreateReservation(
        DateRange period,
        Guid? propertyId = null,
        Guid? roomTypeId = null,
        string guestName = "Test Guest"
    )
    {
        return new Reservation(
            Guid.NewGuid(),
            propertyId ?? Guid.NewGuid(),
            roomTypeId ?? Guid.NewGuid(),
            period,
            new TimeOnly( 14, 0 ),
            new TimeOnly( 12, 0 ),
            guestName,
            "+71234567890",
            DefaultDailyPrice
        );
    }

    [Fact]
    public void Add_Then_Get_ReturnsSameInstance()
    {
        Reservation reservation = CreateReservation(
            new DateRange(
                new DateOnly( 2026, 6, 1 ),
                new DateOnly( 2026, 6, 4 )
            )
        );

        _repository.Add( reservation );
        Reservation? loaded = _repository.Get( reservation.Id );

        Assert.Same( reservation, loaded );
    }

    [Fact]
    public void Search_ReturnsAllReservations()
    {
        Reservation first = CreateReservation(
            new DateRange(
                new DateOnly( 2026, 6, 1 ),
                new DateOnly( 2026, 6, 4 )
            )
        );

        Reservation second = CreateReservation(
            new DateRange(
                new DateOnly( 2026, 7, 1 ),
                new DateOnly( 2026, 7, 5 )
            )
        );

        _repository.Add( first );
        _repository.Add( second );

        IReadOnlyCollection<Reservation> result = _repository.Search( new ReservationFilter() );

        Assert.Equal( 2, result.Count );
    }

    [Fact]
    public void Search_ByPropertyId_ReturnsOnlyMatching()
    {
        Guid targetProperty = Guid.NewGuid();
        Reservation matching = CreateReservation(
            new DateRange(
                new DateOnly( 2026, 6, 1 ),
                new DateOnly( 2026, 6, 4 )
            ),
            propertyId: targetProperty
        );

        Reservation other = CreateReservation(
            new DateRange(
                new DateOnly( 2026, 6, 1 ),
                new DateOnly( 2026, 6, 4 )
            )
        );

        _repository.Add( matching );
        _repository.Add( other );

        IReadOnlyCollection<Reservation> result = _repository.Search(
            new ReservationFilter( PropertyId: targetProperty )
        );

        Reservation single = Assert.Single( result );
        Assert.Same( matching, single );
    }

    [Fact]
    public void Search_ByRoomTypeId_ReturnsOnlyMatching()
    {
        Guid targetRoomType = Guid.NewGuid();
        Reservation matching = CreateReservation(
            new DateRange(
                new DateOnly( 2026, 6, 1 ),
                new DateOnly( 2026, 6, 4 )
            ),
            roomTypeId: targetRoomType
        );

        Reservation other = CreateReservation(
            new DateRange(
                new DateOnly( 2026, 6, 1 ),
                new DateOnly( 2026, 6, 4 )
            )
        );

        _repository.Add( matching );
        _repository.Add( other );

        IReadOnlyCollection<Reservation> result = _repository.Search(
            new ReservationFilter( RoomTypeId: targetRoomType )
        );

        Reservation single = Assert.Single( result );
        Assert.Same( matching, single );
    }

    [Fact]
    public void Search_ByGuestName_IsCaseInsensitiveAndPartial()
    {
        Reservation matching = CreateReservation(
            new DateRange(
                new DateOnly( 2026, 6, 1 ),
                new DateOnly( 2026, 6, 4 )
            ),
            guestName: "Ivan Ivanov"
        );

        Reservation other = CreateReservation(
            new DateRange(
                new DateOnly( 2026, 6, 1 ),
                new DateOnly( 2026, 6, 4 )
            ),
            guestName: "Petr Petrov"
        );

        _repository.Add( matching );
        _repository.Add( other );

        IReadOnlyCollection<Reservation> result = _repository.Search(
            new ReservationFilter( GuestName: "ivan" )
        );

        Reservation single = Assert.Single( result );
        Assert.Same( matching, single );
    }

    [Fact]
    public void Search_ByPeriod_ReturnsOverlapping()
    {
        Reservation overlapping = CreateReservation(
            new DateRange(
                new DateOnly( 2026, 6, 1 ),
                new DateOnly( 2026, 6, 10 )
            )
        );

        Reservation disjoint = CreateReservation(
            new DateRange(
                new DateOnly( 2026, 7, 1 ),
                new DateOnly( 2026, 7, 5 )
            )
        );

        _repository.Add( overlapping );
        _repository.Add( disjoint );

        IReadOnlyCollection<Reservation> result = _repository.Search(
            new ReservationFilter(
                Period: new DateRange(
                    new DateOnly( 2026, 6, 5 ),
                    new DateOnly( 2026, 6, 8 )
                )
            )
        );

        Reservation single = Assert.Single( result );
        Assert.Same( overlapping, single );
    }

    [Fact]
    public void Search_CombinedFilters_AppliesAll()
    {
        Guid targetProperty = Guid.NewGuid();
        Reservation matching = CreateReservation(
            new DateRange(
                new DateOnly( 2026, 6, 1 ),
                new DateOnly( 2026, 6, 4 )
            ),
            propertyId: targetProperty,
            guestName: "Ivan Ivanov"
        );

        Reservation wrongProperty = CreateReservation(
            new DateRange(
                new DateOnly( 2026, 6, 1 ),
                new DateOnly( 2026, 6, 4 )
            ),
            guestName: "Ivan Ivanov"
        );

        Reservation wrongName = CreateReservation(
            new DateRange(
                new DateOnly( 2026, 6, 1 ),
                new DateOnly( 2026, 6, 4 )
            ),
            propertyId: targetProperty,
            guestName: "Petr Petrov"
        );

        _repository.Add( matching );
        _repository.Add( wrongProperty );
        _repository.Add( wrongName );

        IReadOnlyCollection<Reservation> result = _repository.Search(
            new ReservationFilter(
                PropertyId: targetProperty,
                GuestName: "ivan"
            )
        );

        Reservation single = Assert.Single( result );
        Assert.Same( matching, single );
    }

    [Fact]
    public void GetOverlapping_OverlappingPeriod_ReturnsReservation()
    {
        DateRange existingPeriod = new DateRange(
            new DateOnly( 2026, 6, 1 ),
            new DateOnly( 2026, 6, 10 )
        );

        Reservation reservation = CreateReservation( existingPeriod );
        _repository.Add( reservation );

        IReadOnlyCollection<Reservation> result = _repository.GetOverlapping(
            reservation.RoomTypeId,
            new DateRange(
                new DateOnly( 2026, 6, 5 ),
                new DateOnly( 2026, 6, 8 )
            )
        );

        Reservation single = Assert.Single( result );
        Assert.Same( reservation, single );
    }

    [Fact]
    public void GetOverlapping_DisjointPeriod_ReturnsEmpty()
    {
        Reservation reservation = CreateReservation(
            new DateRange(
                new DateOnly( 2026, 6, 1 ),
                new DateOnly( 2026, 6, 4 )
            )
        );

        _repository.Add( reservation );

        IReadOnlyCollection<Reservation> result = _repository.GetOverlapping(
            reservation.RoomTypeId,
            new DateRange(
                new DateOnly( 2026, 6, 4 ),
                new DateOnly( 2026, 6, 8 )
            )
        );

        Assert.Empty( result );
    }

    [Fact]
    public void GetOverlapping_DifferentRoomType_ReturnsEmpty()
    {
        Reservation reservation = CreateReservation(
            new DateRange(
                new DateOnly( 2026, 6, 1 ),
                new DateOnly( 2026, 6, 4 )
            )
        );

        _repository.Add( reservation );

        IReadOnlyCollection<Reservation> result = _repository.GetOverlapping(
            Guid.NewGuid(),
            new DateRange(
                new DateOnly( 2026, 6, 1 ),
                new DateOnly( 2026, 6, 4 )
            )
        );

        Assert.Empty( result );
    }

    [Fact]
    public void Update_Existing_DoesNotThrow()
    {
        Reservation reservation = CreateReservation(
            new DateRange(
                new DateOnly( 2026, 6, 1 ),
                new DateOnly( 2026, 6, 4 )
            )
        );

        _repository.Add( reservation );

        reservation.Cancel();
        _repository.Update( reservation );

        Assert.Equal( ReservationStatus.Cancelled, _repository.Get( reservation.Id )!.Status );
    }

    [Fact]
    public void Update_Unknown_ThrowsEntityNotFound()
    {
        Reservation reservation = CreateReservation(
            new DateRange(
                new DateOnly( 2026, 6, 1 ),
                new DateOnly( 2026, 6, 4 )
            )
        );

        Assert.Throws<EntityNotFoundException>( () => _repository.Update( reservation ) );
    }
}