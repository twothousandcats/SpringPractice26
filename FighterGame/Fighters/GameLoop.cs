namespace Fighters;

public class GameLoop : IGameLoop
{
    public GameLoop()
    {
        IsRunning = true;
    }

    public bool IsRunning { get; private set; }

    public void RequestStop() => IsRunning = false;
}