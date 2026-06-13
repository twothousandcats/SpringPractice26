using Domain.ValueObjects;

namespace Domain.Services;

public sealed record SearchCriteria(
    string City,
    DateRange Period,
    int GuestCount,
    decimal? MaxPricePerNight = null
);