namespace CarFactory.Domain;

public class CarBuilder
{
    public Car Build( CarConfiguration carConfiguration )
    {
        return new Car(
            carConfiguration.Brand,
            carConfiguration.BodyType,
            carConfiguration.Color,
            carConfiguration.SteeringPosition,
            carConfiguration.Engine,
            carConfiguration.Transmission
        );
    }
}