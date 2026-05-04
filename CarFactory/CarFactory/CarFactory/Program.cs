using CarFactory.Domain;
using CarFactory.UI;

namespace CarFactory;

public class Program
{
    public static void Main()
    {
        IConsole console = new SystemConsole();
        ICarFactory carFactory = new ConsoleCarFactory(
            console,
            CarCatalog.BodyTypes,
            CarCatalog.Colors,
            CarCatalog.SteeringPositions,
            CarCatalog.Engines,
            CarCatalog.Transmissions
        );

        Car car = carFactory.Create();

        console.WriteLine( "" );
        console.WriteLine( "Created car settings: " );
        console.WriteLine( car.Description() );
    }
}