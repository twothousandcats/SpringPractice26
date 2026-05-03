namespace CarFactory.Domain.Components.Transmission;

public interface ITransmission
{
    string Name { get; }

    int GearCount { get; }

    string Description();
}