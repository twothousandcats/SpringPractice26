namespace CarFactory.Domain;

public interface ICarFactory
{
    Car Create( CarSpec carSpec );
}