namespace CarFactory.Domain.Components.Transmission;

public class AutomaticTransmission : TransmissionBase
{
    public AutomaticTransmission( int gearCount ) : base( "Automatic", gearCount )
    {
    }

    public override string Description()
    {
        return $"Automatic transmission, {GearCount} gears";
    }
}