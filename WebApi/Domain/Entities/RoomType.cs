using System.Diagnostics.CodeAnalysis;
using Domain.ValueObjects;

namespace Domain.Entities;

public sealed class RoomType
{
    public Guid Id { get; }

    public Guid PropertyId { get; }

    public string Name { get; private set; }

    public Money DailyPrice { get; private set; }

    public int MinPersonCount { get; private set; }

    public int MaxPersonCount { get; private set; }

    public int TotalRooms { get; private set; }

    public IReadOnlyCollection<string> Services { get; private set; }

    public IReadOnlyCollection<string> Amenities { get; private set; }

    public RoomType(
        Guid id,
        Guid propertyId,
        string name,
        Money dailyPrice,
        int minPersonCount,
        int maxPersonCount,
        int totalRooms,
        IEnumerable<string> services,
        IEnumerable<string> amenities
    )
    {
        if ( id == Guid.Empty )
        {
            throw new ArgumentException( "Id is required", nameof( id ) );
        }

        if ( propertyId == Guid.Empty )
        {
            throw new ArgumentException( "PropertyId is required", nameof( propertyId ) );
        }

        Id = id;
        PropertyId = propertyId;
        Update( name, dailyPrice, minPersonCount, maxPersonCount, totalRooms, services, amenities );
    }

    // todo: look for a better solution?
    [MemberNotNull( nameof( Name ), nameof( DailyPrice ), nameof( Services ), nameof( Amenities ) )]
    public void Update(
        string name,
        Money dailyPrice,
        int minPersonCount,
        int maxPersonCount,
        int totalRooms,
        IEnumerable<string> services,
        IEnumerable<string> amenities
    )
    {
        string[] servicesCopy = services.ToArray() ?? throw new ArgumentNullException( nameof( services ) );
        string[] amenitiesCopy = amenities.ToArray() ?? throw new ArgumentNullException( nameof( amenities ) );
        Validate( name, dailyPrice, minPersonCount, maxPersonCount, totalRooms, servicesCopy, amenitiesCopy );

        Name = name;
        DailyPrice = dailyPrice;
        MinPersonCount = minPersonCount;
        MaxPersonCount = maxPersonCount;
        TotalRooms = totalRooms;
        Services = servicesCopy;
        Amenities = amenitiesCopy;
    }

    public bool IsFits( int guests )
    {
        return guests >= MinPersonCount && guests <= MaxPersonCount;
    }

    private static void Validate(
        string name,
        Money dailyPrice,
        int minPersonCount,
        int maxPersonCount,
        int totalRooms,
        IReadOnlyCollection<string> services,
        IReadOnlyCollection<string> amenities
    )
    {
        if ( string.IsNullOrWhiteSpace( name ) )
        {
            throw new ArgumentException( "Name is required.", nameof( name ) );
        }

        ArgumentNullException.ThrowIfNull( dailyPrice );
        if ( dailyPrice.Amount <= 0 )
        {
            throw new ArgumentOutOfRangeException( nameof( dailyPrice ), "Daily price must be positive." );
        }

        if ( minPersonCount <= 0 )
        {
            throw new ArgumentOutOfRangeException( nameof( minPersonCount ), "Min person count must be positive." );
        }

        if ( maxPersonCount < minPersonCount )
        {
            throw new ArgumentException(
                "Max person count must be greater than or equal to min.", nameof( maxPersonCount )
            );
        }

        if ( totalRooms <= 0 )
        {
            throw new ArgumentOutOfRangeException( nameof( totalRooms ), "Total rooms must be positive." );
        }

        if ( services.Any( string.IsNullOrWhiteSpace ) )
        {
            throw new ArgumentException( "Services must not contain empty entries.", nameof( services ) );
        }

        if ( amenities.Any( string.IsNullOrWhiteSpace ) )
        {
            throw new ArgumentException( "Amenities must not contain empty entries.", nameof( amenities ) );
        }
    }
}