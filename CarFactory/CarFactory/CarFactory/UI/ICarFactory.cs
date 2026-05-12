using CarFactory.Domain;

namespace CarFactory.UI;

public interface ICarFactory
{
    Car Create();
}