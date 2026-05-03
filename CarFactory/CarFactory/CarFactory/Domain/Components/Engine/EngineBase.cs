namespace CarFactory.Domain.Components.Engine;

public abstract class EngineBase : IEngine
{
    private readonly string _name;

    private readonly int _maxSpeed;

    protected EngineBase( string name, int maxSpeed )
    {
        _name = name;
        _maxSpeed = maxSpeed;
    }

    public string Name => _name;

    public int MaxSpeed => _maxSpeed;

    public abstract string Description();
}