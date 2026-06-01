using CarFactory.Domain.Components.BodyTypes;
using CarFactory.Domain.Components.Colors;
using CarFactory.Domain.Components.Engine;
using CarFactory.Domain.Components.SteeringPosition;
using CarFactory.Domain.Components.Transmission;

namespace CarFactory.Domain;

public record CarSpec(
    string Brand,
    IBodyType BodyType,
    ICarColor Color,
    ISteeringPosition SteeringPosition,
    IEngine Engine,
    ITransmission Transmission
);