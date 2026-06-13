using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Domain.ValueObjects;
using Infrastructure.Repositories;

namespace Infrastructure.Tests.Repositories;

public class InMemoryReservationRepositoryTests
{
    [Fact]
    public void Add_Then_Get_ReturnsSameInstance()
    {
        // Arrange
        DateRange period = new DateRange(
            new DateOnly( 2026, 1, 1 ),
            new DateOnly( 2026, 1, 3 )
        );

        Reservation reservation = new Reservation(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            period,
            new TimeOnly( 14, 0 ),
            new TimeOnly( 12, 0 ),
            "Test name",
            "+88005553535",
            new Money( 100m, "Test currency" )
        );

        InMemoryReservationRepository sut = new InMemoryReservationRepository();

        // Act
        sut.Add( reservation );
        Reservation? loaded = sut.Get( reservation.Id );

        // Assert
        Assert.Same( reservation, loaded );
    }

    [Fact]
    public void Search_ReturnsAllReservations()
    {
        // Arrange
        Reservation firstReservation = new Reservation(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            new DateRange(
                new DateOnly( 2026, 1, 1 ),
                new DateOnly( 2026, 1, 3 )
            ),
            new TimeOnly( 14, 0 ),
            new TimeOnly( 12, 0 ),
            "Test name",
            "+88005553535",
            new Money( 100m, "Test currency" )
        );

        Reservation secondReservation = new Reservation(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            new DateRange(
                new DateOnly( 2026, 1, 1 ),
                new DateOnly( 2026, 1, 4 )
            ),
            new TimeOnly( 14, 0 ),
            new TimeOnly( 12, 0 ),
            "Test name",
            "+88005553535",
            new Money( 100m, "Test currency" )
        );

        InMemoryReservationRepository sut = new InMemoryReservationRepository();
        sut.Add( firstReservation );
        sut.Add( secondReservation );

        // Act
        IReadOnlyCollection<Reservation> result = sut.Search( new ReservationFilter() );

        // Assert
        Assert.Equal( 2, result.Count );
    }

    [Fact]
    public void Search_ByPropertyId_ReturnsOnlyMatching()
    {
        // Arrange
        Guid targetProperty = Guid.NewGuid();
        Reservation matching = new Reservation(
            Guid.NewGuid(),
            targetProperty,
            Guid.NewGuid(),
            new DateRange(
                new DateOnly( 2026, 1, 1 ),
                new DateOnly( 2026, 1, 3 )
            ),
            new TimeOnly( 14, 0 ),
            new TimeOnly( 12, 0 ),
            "Test name",
            "+88005553535",
            new Money( 100m, "Test currency" )
        );

        Reservation other = new Reservation(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            new DateRange(
                new DateOnly( 2026, 6, 1 ),
                new DateOnly( 2026, 6, 4 )
            ),
            new TimeOnly( 14, 0 ),
            new TimeOnly( 12, 0 ),
            "Test name",
            "+88005553535",
            new Money( 100m, "Test currency" )
        );

        InMemoryReservationRepository sut = new InMemoryReservationRepository();
        sut.Add( matching );
        sut.Add( other );
        ReservationFilter reservationFilter = new ReservationFilter( PropertyId: targetProperty );

        // Act
        IReadOnlyCollection<Reservation> result = sut.Search( reservationFilter );

        // Assert
        Reservation single = Assert.Single( result );
        Assert.Same( matching, single );
    }

    [Fact]
    public void Search_ByRoomTypeId_ReturnsOnlyMatching()
    {
        // Arrange
        Guid targetRoomType = Guid.NewGuid();
        Reservation matching = new Reservation(
            Guid.NewGuid(),
            Guid.NewGuid(),
            targetRoomType,
            new DateRange(
                new DateOnly( 2026, 1, 1 ),
                new DateOnly( 2026, 1, 3 )
            ),
            new TimeOnly( 14, 0 ),
            new TimeOnly( 12, 0 ),
            "Test name",
            "+88005553535",
            new Money( 100m, "Test currency" )
        );

        Reservation other = new Reservation(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            new DateRange(
                new DateOnly( 2026, 6, 1 ),
                new DateOnly( 2026, 6, 4 )
            ),
            new TimeOnly( 14, 0 ),
            new TimeOnly( 12, 0 ),
            "Test name",
            "+88005553535",
            new Money( 100m, "Test currency" )
        );

        InMemoryReservationRepository sut = new InMemoryReservationRepository();
        sut.Add( matching );
        sut.Add( other );
        ReservationFilter reservationFilter = new ReservationFilter( RoomTypeId: targetRoomType );

        // Act
        IReadOnlyCollection<Reservation> result = sut.Search( reservationFilter );

        // Assert
        Reservation single = Assert.Single( result );
        Assert.Same( matching, single );
    }

    [Fact]
    public void Search_ByGuestName_IsCaseInsensitiveAndPartial()
    {
        // Arrange
        Reservation matching = new Reservation(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            new DateRange(
                new DateOnly( 2026, 1, 1 ),
                new DateOnly( 2026, 1, 3 )
            ),
            new TimeOnly( 14, 0 ),
            new TimeOnly( 12, 0 ),
            "Ivan",
            "+88005553535",
            new Money( 100m, "Test currency" )
        );

        Reservation other = new Reservation(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            new DateRange(
                new DateOnly( 2026, 6, 1 ),
                new DateOnly( 2026, 6, 4 )
            ),
            new TimeOnly( 14, 0 ),
            new TimeOnly( 12, 0 ),
            "Petr",
            "+88005553535",
            new Money( 100m, "Test currency" )
        );

        InMemoryReservationRepository sut = new InMemoryReservationRepository();

        sut.Add( matching );
        sut.Add( other );
        ReservationFilter reservationFilter = new ReservationFilter( GuestName: "ivan" );

        // Act
        IReadOnlyCollection<Reservation> result = sut.Search( reservationFilter );

        // Assert
        Reservation single = Assert.Single( result );
        Assert.Same( matching, single );
    }

    [Fact]
    public void Search_ByPeriod_ReturnsOverlapping()
    {
        // Arrange
        Reservation overlapping = new Reservation(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            new DateRange(
                new DateOnly( 2026, 1, 1 ),
                new DateOnly( 2026, 1, 10 )
            ),
            new TimeOnly( 14, 0 ),
            new TimeOnly( 12, 0 ),
            "Test name",
            "+88005553535",
            new Money( 100m, "Test currency" )
        );

        Reservation disjoint = new Reservation(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            new DateRange(
                new DateOnly( 2026, 1, 1 ),
                new DateOnly( 2026, 1, 5 )
            ),
            new TimeOnly( 14, 0 ),
            new TimeOnly( 12, 0 ),
            "Test name",
            "+88005553535",
            new Money( 100m, "Test currency" )
        );

        InMemoryReservationRepository sut = new InMemoryReservationRepository();

        sut.Add( overlapping );
        sut.Add( disjoint );
        ReservationFilter reservationFilter = new ReservationFilter(
            Period: new DateRange(
                new DateOnly( 2026, 1, 5 ),
                new DateOnly( 2026, 1, 8 )
            )
        );

        // Act
        IReadOnlyCollection<Reservation> result = sut.Search( reservationFilter );

        // Assert
        Reservation single = Assert.Single( result );
        Assert.Same( overlapping, single );
    }

    [Fact]
    public void Search_CombinedFilters_AppliesAll()
    {
        // Arrange
        Guid targetProperty = Guid.NewGuid();
        Reservation matching = new Reservation(
            Guid.NewGuid(),
            targetProperty,
            Guid.NewGuid(),
            new DateRange(
                new DateOnly( 2026, 6, 1 ),
                new DateOnly( 2026, 6, 4 )
            ),
            new TimeOnly( 10, 20, 30 ),
            new TimeOnly( 10, 20, 30 ),
            "Ivan Ivanov",
            "88005553535",
            new Money( 100m, "Test currency" )
        );

        Reservation wrongProperty = new Reservation(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            new DateRange(
                new DateOnly( 2026, 6, 1 ),
                new DateOnly( 2026, 6, 4 )
            ),
            new TimeOnly( 10, 20, 30 ),
            new TimeOnly( 10, 20, 30 ),
            "Ivan Ivanov",
            "88005553535",
            new Money( 100m, "Test currency" )
        );

        Reservation wrongName = new Reservation(
            Guid.NewGuid(),
            targetProperty,
            Guid.NewGuid(),
            new DateRange(
                new DateOnly( 2026, 6, 1 ),
                new DateOnly( 2026, 6, 4 )
            ),
            new TimeOnly( 10, 20, 30 ),
            new TimeOnly( 10, 20, 30 ),
            "Petr Petrov",
            "88005553535",
            new Money( 100m, "Test currency" )
        );

        InMemoryReservationRepository sut = new InMemoryReservationRepository();
        sut.Add( matching );
        sut.Add( wrongProperty );
        sut.Add( wrongName );
        ReservationFilter reservationFilter = new ReservationFilter(
            PropertyId: targetProperty,
            GuestName: "ivan"
        );

        // Act
        IReadOnlyCollection<Reservation> result = sut.Search( reservationFilter );

        // Assert
        Reservation single = Assert.Single( result );
        Assert.Same( matching, single );
    }

    [Fact]
    public void GetOverlapping_OverlappingPeriod_ReturnsReservation()
    {
        // Arrange
        DateRange existingPeriod = new DateRange(
            new DateOnly( 2026, 6, 1 ),
            new DateOnly( 2026, 6, 10 )
        );

        Reservation reservation = new Reservation(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            existingPeriod,
            new TimeOnly( 10, 20, 30 ),
            new TimeOnly( 10, 20, 30 ),
            "Ivan Ivanov",
            "88005553535",
            new Money( 100m, "Test currency" )
        );

        InMemoryReservationRepository sut = new InMemoryReservationRepository();
        sut.Add( reservation );

        // Act
        IReadOnlyCollection<Reservation> result = sut.GetOverlapping(
            reservation.RoomTypeId,
            new DateRange(
                new DateOnly( 2026, 6, 5 ),
                new DateOnly( 2026, 6, 8 )
            )
        );

        // Assert
        Reservation single = Assert.Single( result );
        Assert.Same( reservation, single );
    }

    [Fact]
    public void GetOverlapping_DisjointPeriod_ReturnsEmpty()
    {
        // Arrange
        Reservation reservation = new Reservation(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            new DateRange(
                new DateOnly( 2026, 6, 1 ),
                new DateOnly( 2026, 6, 3 )
            ),
            new TimeOnly( 10, 20, 30 ),
            new TimeOnly( 10, 20, 30 ),
            "Ivan Ivanov",
            "88005553535",
            new Money( 100m, "Test currency" )
        );

        InMemoryReservationRepository sut = new InMemoryReservationRepository();
        sut.Add( reservation );

        // Act
        IReadOnlyCollection<Reservation> result = sut.GetOverlapping(
            reservation.RoomTypeId,
            new DateRange(
                new DateOnly( 2026, 6, 4 ),
                new DateOnly( 2026, 6, 8 )
            )
        );

        // Assert
        Assert.Empty( result );
    }

    [Fact]
    public void GetOverlapping_DifferentRoomType_ReturnsEmpty()
    {
        // Arrange
        Reservation reservation = new Reservation(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            new DateRange(
                new DateOnly( 2026, 6, 1 ),
                new DateOnly( 2026, 6, 3 )
            ),
            new TimeOnly( 10, 20, 30 ),
            new TimeOnly( 10, 20, 30 ),
            "Ivan Ivanov",
            "88005553535",
            new Money( 100m, "Test currency" )
        );

        InMemoryReservationRepository sut = new InMemoryReservationRepository();
        sut.Add( reservation );

        // Act
        IReadOnlyCollection<Reservation> result = sut.GetOverlapping(
            Guid.NewGuid(),
            new DateRange(
                new DateOnly( 2026, 6, 1 ),
                new DateOnly( 2026, 6, 4 )
            )
        );

        // Assert
        Assert.Empty( result );
    }

    [Fact]
    public void Update_Existing_DoesNotThrow()
    {
        // Arrange
        Reservation reservation = new Reservation(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            new DateRange(
                new DateOnly( 2026, 6, 1 ),
                new DateOnly( 2026, 6, 3 )
            ),
            new TimeOnly( 10, 20, 30 ),
            new TimeOnly( 10, 20, 30 ),
            "Ivan Ivanov",
            "88005553535",
            new Money( 100m, "Test currency" )
        );

        InMemoryReservationRepository sut = new InMemoryReservationRepository();
        sut.Add( reservation );

        // Act
        reservation.Cancel();
        sut.Update( reservation );

        // Assert
        Assert.Equal( ReservationStatus.Cancelled, sut.Get( reservation.Id )!.Status );
    }

    [Fact]
    public void Update_Unknown_ThrowsEntityNotFound()
    {
        // Arrange
        Reservation reservation = new Reservation(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            new DateRange(
                new DateOnly( 2026, 6, 1 ),
                new DateOnly( 2026, 6, 3 )
            ),
            new TimeOnly( 10, 20, 30 ),
            new TimeOnly( 10, 20, 30 ),
            "Ivan Ivanov",
            "88005553535",
            new Money( 100m, "Test currency" )
        );

        InMemoryReservationRepository sut = new InMemoryReservationRepository();

        // Act, Assert
        Assert.Throws<EntityNotFoundException>( () => sut.Update( reservation ) );
    }
}