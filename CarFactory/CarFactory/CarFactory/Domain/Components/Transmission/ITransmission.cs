namespace CarFactory.Domain.Components.Transmission;

public interface ITransmission : IName
{
    int GearCount { get; }

    string Description();
}