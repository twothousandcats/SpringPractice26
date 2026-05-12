namespace CarFactory.Domain.Components.Transmission;

public interface ITransmission : INamed
{
    int GearCount { get; }

    string Description();
}