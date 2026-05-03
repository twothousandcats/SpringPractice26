namespace CarFactory.Domain.Components.Transmission;

public abstract class TransmissionBase : ITransmission
{
    private readonly string _name;

    private readonly int _gearCount;

    protected TransmissionBase( string name, int gearCount )
    {
        _name = name;
        _gearCount = gearCount;
    }

    public string Name => _name;

    public int GearCount => _gearCount;

    public abstract string Description();
}