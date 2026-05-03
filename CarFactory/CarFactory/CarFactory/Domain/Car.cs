using CarFactory.Domain.Components.Engine;
using CarFactory.Domain.Components.Transmission;
using CarFactory.Domain.Enums;

namespace CarFactory.Domain;

public class Car
{
    private readonly string _brand;

    private readonly BodyType _bodyType;

    private readonly CarColor _color;

    private readonly SteeringPosition _steeringPosition;

    private readonly IEngine _engine;

    private readonly ITransmission _transmission;

    public Car(
        string brand,
        BodyType bodyType,
        CarColor color,
        SteeringPosition steeringPosition,
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

    public BodyType BodyType => _bodyType;

    public CarColor Color => _color;

    public SteeringPosition SteeringPosition => _steeringPosition;

    public IEngine Engine => _engine;

    public ITransmission Transmission => _transmission;

    public int MaxSpeed => _engine.MaxSpeed;

    public int GearCount => _transmission.GearCount;

    public string Description()
    {
        return string.Join(
            Environment.NewLine,
            $"Brand: {_brand}",
            $"BodyType: {_bodyType}",
            $"Color: {_color}",
            $"Steering Position: {_steeringPosition}",
            $"Engine: {_engine.Description()}",
            $"Transmission: {_transmission.Description()}",
            $"Max Speed: {MaxSpeed}",
            $"Gear Count: {GearCount}"
        );
    }
}