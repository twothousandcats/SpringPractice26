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

    private static Reservation CreateReservation( DateRange period )
    {
        return new Reservation(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            period,
            new TimeOnly( 14, 0 ),
            new TimeOnly( 12, 0 ),
            "Test Guest",
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

        IReadOnlyCollection<Reservation> result = _repository.Search();

        Assert.Equal( 2, result.Count );
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