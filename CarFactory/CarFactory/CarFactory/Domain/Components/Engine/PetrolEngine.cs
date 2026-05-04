namespace CarFactory.Domain.Components.Engine;

public class PetrolEngine : EngineBase
{
    private readonly double _displacement;

    public PetrolEngine( double displacement, int maxSpeed ) : base( $"Petrol {displacement:F1}L", maxSpeed )
    {
        if ( displacement <= 0 )
        {
            throw new ArgumentOutOfRangeException( nameof( displacement ), "Displacement must be positive" );
        }

        _displacement = displacement;
    }

    public override string Description()
    {
        return $"Petrol engine {_displacement:F1}L, max speed {MaxSpeed} km/h";
    }
}