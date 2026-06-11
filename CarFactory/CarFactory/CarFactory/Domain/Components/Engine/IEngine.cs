namespace CarFactory.Domain.Components.Engine;

public interface IEngine : INamed
{
    int MaxSpeed { get; }

    string Description();
}