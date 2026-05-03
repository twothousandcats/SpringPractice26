namespace CarFactory.Domain.Components;

public interface IEngine
{
    string Name { get; }

    int MaxSpeed { get; }

    string Description();
}