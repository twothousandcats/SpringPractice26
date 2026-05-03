namespace CarFactory.Domain.Components.Engine;

public interface IEngine
{
    string Name { get; }

    int MaxSpeed { get; }

    string Description();
}