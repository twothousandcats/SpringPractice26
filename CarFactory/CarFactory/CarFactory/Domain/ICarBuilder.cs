namespace CarFactory.Domain
{
    public interface ICarBuilder
    {
        Car Build( CarConfiguration carConfiguration );
    }
}