namespace CarFactory.Domain.Components.Engine;

public class ElectricEngine : EngineBase
{
    private readonly int _batteryKwh;

    public ElectricEngine( int batteryKwh, int maxSpeed ) : base( "Electric", maxSpeed )
    {
        if ( batteryKwh <= 0 )
        {
            throw new ArgumentOutOfRangeException( nameof( batteryKwh ), "Battery capacity must be positive" );
        }

        _batteryKwh = batteryKwh;
    }

    public override string Description()
    {
        return $"Electric engine {_batteryKwh} kWh, max speed {MaxSpeed} km/h";
    }
}