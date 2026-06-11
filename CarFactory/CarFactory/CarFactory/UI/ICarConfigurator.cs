using CarFactory.Domain;

namespace CarFactory.UI;

public interface ICarConfigurator
{
    CarSpec Configure();
}