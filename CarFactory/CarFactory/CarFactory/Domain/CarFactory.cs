using CarFactory.Domain.Components.Engine;
using CarFactory.Domain.Components.Transmission;

namespace CarFactory.Domain;

public class CarFactory : ICarFactory
{
    private const int HighTopSpeed = 280;

    private const int MinGearsForHighTopSpeed = 6;

    public Car Create( CarSpec carSpec )
    {
        ValidateSpec(
            carSpec.Engine,
            carSpec.Transmission
        );

        return Car.Create(
            carSpec.Brand,
            carSpec.BodyType,
            carSpec.Color,
            carSpec.SteeringPosition,
            carSpec.Engine,
            carSpec.Transmission
        );
    }

    private static void ValidateSpec( IEngine engine, ITransmission transmission )
    {
        if ( engine.MaxSpeed >= HighTopSpeed && transmission.GearCount < MinGearsForHighTopSpeed )
        {
            throw new ArgumentException(
                $"Engine with top speed {engine.MaxSpeed} km/h requires at least " +
                $"{MinGearsForHighTopSpeed} gears, but transmission has {transmission.GearCount}",
                nameof( transmission )
            );
        }
    }
}