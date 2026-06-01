namespace CarFactory.Domain.Components.Transmission;

public abstract class TransmissionBase : ITransmission
{
    private readonly string _name;

    private readonly int _gearCount;

    protected TransmissionBase( string name, int gearCount )
    {
        if ( string.IsNullOrWhiteSpace( name ) )
        {
            throw new ArgumentNullException( nameof( name ), "Name cant be empty" );
        }

        if ( gearCount <= 0 )
        {
            throw new ArgumentOutOfRangeException(
                nameof( gearCount ),
                "Gear count should be greater than zero"
            );
        }

        _name = name;
        _gearCount = gearCount;
    }

    public string Name => _name;

    public int GearCount => _gearCount;

    public abstract string Description();
}