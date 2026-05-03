using CarFactory.Domain.Enums;
using CarFactory.Domain.Components.Engine;
using CarFactory.Domain.Components.Transmission;

namespace CarFactory.Domain;

public class CarConfiguration
{
    public required string Brand { get; init; }

    public required BodyType BodyType { get; init; }

    public required CarColor Color { get; init; }

    public required SteeringPosition SteeringPosition { get; init; }

    public required IEngine Engine { get; init; }

    public required ITransmission Transmission { get; init; }
}