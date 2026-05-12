using CarFactory.Domain.Components.BodyTypes;
using CarFactory.Domain.Components.Colors;
using CarFactory.Domain.Components.Engine;
using CarFactory.Domain.Components.SteeringPosition;
using CarFactory.Domain.Components.Transmission;

namespace CarFactory.UI;

public static class CarCatalog
{
    public static IReadOnlyList<IBodyType> BodyTypes { get; } =
    [
        new Sedan(),
        new Hatchback(),
        new Suv(),
    ];

    public static IReadOnlyList<ICarColor> Colors { get; } =
    [
        new Black(),
        new White(),
        new Red(),
        new Green(),
        new Blue(),
    ];

    public static IReadOnlyList<ISteeringPosition> SteeringPositions { get; } =
    [
        new Left(),
        new Right(),
    ];

    public static IReadOnlyList<IEngine> Engines { get; } =
    [
        new PetrolEngine( 1.5, 180 ),
        new PetrolEngine( 2.0, 220 ),
        new PetrolEngine( 3.0, 260 ),
        new ElectricEngine( 50, 180 ),
        new ElectricEngine( 100, 250 ),
    ];

    public static IReadOnlyList<ITransmission> Transmissions { get; } =
    [
        new ManualTransmission( 5 ),
        new ManualTransmission( 6 ),
        new AutomaticTransmission( 6 ),
        new AutomaticTransmission( 8 ),
    ];
}