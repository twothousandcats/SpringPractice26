namespace Fighters;

public class ApplicationLifetime : IApplicationLifetime
{
    public bool ShouldStop { get; private set; }
    public void RequestStop() => ShouldStop = true;
}
