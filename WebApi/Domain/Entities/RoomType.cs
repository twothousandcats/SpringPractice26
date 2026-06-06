using WebApi.Domain.ValueObjects;

namespace WebApi.Domain.Entities;

public class RoomType
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
            throw new ArgumentException( "PropertyId is required", nameof( id ) );
        }

        (
            string name,
            Money price,
            int min,
            int max,
            int total,
            IReadOnlyCollection<string> servicesCopy,
            IReadOnlyCollection<string> amenitiesCopy
            ) validated = Validate(
                name,
                dailyPrice,
                minPersonCount,
                maxPersonCount,
                totalRooms,
                services,
                amenities
            );

        Id = id;
        PropertyId = propertyId;
        Name = validated.name;
        DailyPrice = validated.price;
        MinPersonCount = validated.min;
        MaxPersonCount = validated.max;
        TotalRooms = validated.total;
        Services = validated.servicesCopy;
        Amenities = validated.amenitiesCopy;
    }

    public void Update( string name,
        Money dailyPrice,
        int minPersonCount,
        int maxPersonCount,
        int totalRooms,
        IEnumerable<string> services,
        IEnumerable<string> amenities
    )
    {
        (
            string name,
            Money price,
            int min,
            int max,
            int total,
            IReadOnlyCollection<string> servicesCopy,
            IReadOnlyCollection<string> amenitiesCopy
            ) validated = Validate(
                name,
                dailyPrice,
                minPersonCount,
                maxPersonCount,
                totalRooms,
                services,
                amenities
            );

        Name = validated.name;
        DailyPrice = validated.price;
        MinPersonCount = validated.min;
        MaxPersonCount = validated.max;
        TotalRooms = validated.total;
        Services = validated.servicesCopy;
        Amenities = validated.amenitiesCopy;
    }

    public bool IsFits( int guests )
    {
        return guests >= MinPersonCount && guests <= MaxPersonCount;
    }

    private static (
        string name,
        Money price,
        int min,
        int max,
        int total,
        IReadOnlyCollection<string> servicesCopy,
        IReadOnlyCollection<string> amenitiesCopy
        )
        Validate(
            string name,
            Money dailyPrice,
            int minPersonCount,
            int maxPersonCount,
            int totalRooms,
            IEnumerable<string> services,
            IEnumerable<string> amenities )
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

        ArgumentNullException.ThrowIfNull( services );
        ArgumentNullException.ThrowIfNull( amenities );

        string[] servicesCopy = services.ToArray();
        string[] amenitiesCopy = amenities.ToArray();

        if ( servicesCopy.Any( string.IsNullOrWhiteSpace ) )
        {
            throw new ArgumentException( "Services must not contain empty entries.", nameof( services ) );
        }

        if ( amenitiesCopy.Any( string.IsNullOrWhiteSpace ) )
        {
            throw new ArgumentException( "Amenities must not contain empty entries.", nameof( amenities ) );
        }

        return ( name, dailyPrice, minPersonCount, maxPersonCount, totalRooms, servicesCopy, amenitiesCopy );
    }
}