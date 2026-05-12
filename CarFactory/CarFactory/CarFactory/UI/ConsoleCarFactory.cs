using CarFactory.Domain;
using CarFactory.Domain.Components.BodyTypes;
using CarFactory.Domain.Components.Colors;
using CarFactory.Domain.Components.Engine;
using CarFactory.Domain.Components.SteeringPosition;
using CarFactory.Domain.Components.Transmission;

namespace CarFactory.UI;

public class ConsoleCarFactory : ICarFactory
{
    private readonly IConsole _console;

    private readonly IReadOnlyList<IBodyType> _bodyTypes;

    private readonly IReadOnlyList<ICarColor> _carColors;

    private readonly IReadOnlyList<ISteeringPosition> _steeringPositions;

    private readonly IReadOnlyList<IEngine> _engines;

    private readonly IReadOnlyList<ITransmission> _transmissions;

    public ConsoleCarFactory(
        IConsole console,
        IReadOnlyList<IBodyType> bodyTypes,
        IReadOnlyList<ICarColor> carColors,
        IReadOnlyList<ISteeringPosition> steeringPositions,
        IReadOnlyList<IEngine> engines,
        IReadOnlyList<ITransmission> transmissions
    )
    {
        _console = console;
        _bodyTypes = bodyTypes;
        _carColors = carColors;
        _steeringPositions = steeringPositions;
        _engines = engines;
        _transmissions = transmissions;
    }

    public Car Create()
    {
        string brand = ReadBrand();
        try
        {
            IBodyType bodyType = ReadFromList( "Choose body type: ", _bodyTypes );
            ICarColor carColor = ReadFromList( "Choose color: ", _carColors );
            ISteeringPosition steeringPosition = ReadFromList( "Choose steering position: ", _steeringPositions );
            IEngine engine = ReadFromList( "Choose engine: ", _engines );
            ITransmission transmission = ReadFromList( "Choose transmission: ", _transmissions );

            return new Car(
                brand,
                bodyType,
                carColor,
                steeringPosition,
                engine,
                transmission
            );
        }
        catch ( Exception ex )
        {
            Console.WriteLine( ex.Message );
            throw;
        }
    }

    private string ReadBrand()
    {
        while ( true )
        {
            _console.WriteLine( "Enter a brand: " );
            string? input = _console.ReadLine();
            if ( !string.IsNullOrWhiteSpace( input ) )
            {
                return input.Trim();
            }

            _console.WriteLine( "Brand cant be empty!" );
        }
    }

    private T ReadFromList<T>( string title, IReadOnlyList<T> options ) where T : INamed
    {
        if ( options.Count == 0 )
        {
            throw new ArgumentException( "have to be not empty", nameof( options ) );
        }

        while ( true )
        {
            _console.WriteLine( title );
            for ( int i = 0; i < options.Count; i++ )
            {
                _console.WriteLine( $"{i + 1} - {options[ i ].Name}" );
            }

            string? input = _console.ReadLine();
            if ( int.TryParse( input, out int choice ) && choice >= 1 && choice <= options.Count )
            {
                return options[ choice - 1 ];
            }

            _console.WriteLine( "Invalid option! Try again." );
        }
    }
}