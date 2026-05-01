namespace Fighters
{
    public interface IApplicationLifetime
    {
        bool ShouldStop { get; }
        void RequestStop();
    }
}
