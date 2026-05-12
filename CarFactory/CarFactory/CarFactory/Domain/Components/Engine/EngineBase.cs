namespace CarFactory.Domain.Components.Engine;

public abstract class EngineBase : IEngine
{
    private readonly string _name;

    private readonly int _maxSpeed;

    protected EngineBase( string name, int maxSpeed )
    {
        if ( string.IsNullOrWhiteSpace( name ) )
        {
            throw new ArgumentNullException( nameof( name ), "Name cant be empty" );
        }

        if ( maxSpeed <= 0 )
        {
            throw new ArgumentOutOfRangeException(
                nameof( maxSpeed ),
                "Max speed should be greater than zero"
            );
        }

        _name = name;
        _maxSpeed = maxSpeed;
    }

    public string Name => _name;

    public int MaxSpeed => _maxSpeed;

    public abstract string Description();
}