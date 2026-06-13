using Domain.Entities;
using Domain.Repositories;
using Domain.ValueObjects;

namespace Domain.Services;

public sealed class SearchService : ISearchService
{
    private readonly IPropertyRepository _propertyRepository;

    private readonly IRoomTypeRepository _roomTypeRepository;

    private readonly IAvailabilityChecker _availabilityChecker;

    public SearchService( IPropertyRepository propertyRepository,
        IRoomTypeRepository roomTypeRepository,
        IAvailabilityChecker availabilityChecker
    )
    {
        _propertyRepository = propertyRepository;
        _roomTypeRepository = roomTypeRepository;
        _availabilityChecker = availabilityChecker;
    }

    public IReadOnlyCollection<SearchVariant> Search( SearchCriteria searchCriteria )
    {
        IReadOnlyCollection<Property> properties = _propertyRepository.GetByCity( searchCriteria.City );
        List<SearchVariant> variants = new List<SearchVariant>();

        foreach ( Property property in properties )
        {
            IReadOnlyCollection<RoomType> roomTypes = _roomTypeRepository.GetByProperty( property.Id );
            foreach ( RoomType roomType in roomTypes )
            {
                if ( !IsMatch( roomType, searchCriteria ) )
                {
                    continue;
                }

                if ( !_availabilityChecker.CanBook( roomType, searchCriteria.Period ) )
                {
                    continue;
                }

                Money totalForPeriod = roomType.DailyPrice.Multiply( searchCriteria.Period.Nights );
                variants.Add(
                    new SearchVariant(
                        property,
                        roomType,
                        totalForPeriod
                    )
                );
            }
        }

        return variants;
    }

    private bool IsMatch( RoomType roomType, SearchCriteria searchCriteria )
    {
        if ( !roomType.IsFits( searchCriteria.GuestCount ) )
        {
            return false;
        }

        if ( searchCriteria.MaxPricePerNight.HasValue &&
             searchCriteria.MaxPricePerNight.Value < roomType.DailyPrice.Amount )
        {
            return false;
        }

        return true;
    }
}