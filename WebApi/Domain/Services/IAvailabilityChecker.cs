using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Services;

public interface IAvailabilityChecker
{
    bool CanBook( RoomType roomType, DateRange period );
}