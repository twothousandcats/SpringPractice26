using CarFactory.Domain;

namespace CarFactory;

public class CarProductionLine
{
    private readonly ICarBuilder _carBuilder;

    public CarProductionLine( ICarBuilder carBuilder )
    {
        _carBuilder = carBuilder;
    }

    public Car Produce( CarConfiguration carConfig )
    {
        return _carBuilder.Build( carConfig );
    }
}