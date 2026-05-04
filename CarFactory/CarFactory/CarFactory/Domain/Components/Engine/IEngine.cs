namespace CarFactory.Domain.Components.Engine;

public interface IEngine : IName
{
    int MaxSpeed { get; }

    string Description();
}