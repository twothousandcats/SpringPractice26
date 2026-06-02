namespace Fighters;

public interface IGameLoop
{
    bool IsRunning { get; }

    void RequestStop();
}