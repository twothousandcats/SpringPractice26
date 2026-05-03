namespace CarFactory.Domain.Components.Transmission;

public class ManualTransmission : TransmissionBase
{
    public ManualTransmission( int gearCount ) : base( "Manual", gearCount )
    {
    }

    public override string Description()
    {
        return $"Manual transmission, {GearCount} gears";
    }
}