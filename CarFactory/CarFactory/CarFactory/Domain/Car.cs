using CarFactory.Domain.Components.BodyTypes;
using CarFactory.Domain.Components.Colors;
using CarFactory.Domain.Components.Engine;
using CarFactory.Domain.Components.SteeringPosition;
using CarFactory.Domain.Components.Transmission;

namespace CarFactory.Domain;

public class Car
{
    private readonly string _brand;

    private readonly IBodyType _bodyType;

    private readonly ICarColor _color;

    private readonly ISteeringPosition _steeringPosition;

    private readonly IEngine _engine;

    private readonly ITransmission _transmission;

    public Car(
        string brand,
        IBodyType bodyType,
        ICarColor color,
        ISteeringPosition steeringPosition,
        IEngine engine,
        ITransmission transmission
    )
    {
        _brand = brand;
        _bodyType = bodyType;
        _color = color;
        _steeringPosition = steeringPosition;
        _engine = engine;
        _transmission = transmission;
    }

    public string Brand => _brand;

    public IBodyType BodyType => _bodyType;

    public ICarColor Color => _color;

    public ISteeringPosition SteeringPosition => _steeringPosition;

    public IEngine Engine => _engine;

    public ITransmission Transmission => _transmission;

    public int MaxSpeed => _engine.MaxSpeed;

    public int GearCount => _transmission.GearCount;

    public string Description()
    {
        return string.Join(
            Environment.NewLine,
            $"Brand: {_brand}",
            $"BodyType: {_bodyType.Name}",
            $"Color: {_color.Name}",
            $"Steering Position: {_steeringPosition.Name}",
            $"Engine: {_engine.Description()}",
            $"Transmission: {_transmission.Description()}",
            $"Max Speed: {MaxSpeed} km/h",
            $"Gear Count: {GearCount}"
        );
    }
}