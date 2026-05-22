using CarFactory.Domain;
using CarFactory.UI;

namespace CarFactory;

public class Program
{
    public static void Main()
    {
        IConsole console = new SystemConsole();
        ICarConfigurator carConfigurator = new ConsoleCarConfigurator(
            console,
            CarCatalog.BodyTypes,
            CarCatalog.Colors,
            CarCatalog.SteeringPositions,
            CarCatalog.Engines,
            CarCatalog.Transmissions
        );
        CarSpec carSpec = carConfigurator.Configure();

        ICarFactory carFactory = new Domain.CarFactory();
        Car car = carFactory.Create( carSpec );

        console.WriteLine( "" );
        console.WriteLine( "Created car settings: " );
        console.WriteLine( car.Description() );
    }
}