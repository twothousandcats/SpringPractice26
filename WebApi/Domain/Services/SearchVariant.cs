using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Services;

public sealed record SearchVariant(
    Property Property,
    RoomType RoomType,
    Money TotalForPeriod
);