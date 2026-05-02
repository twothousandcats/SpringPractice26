namespace Fighters.Commands;

public class ExitConsoleCommand : IConsoleCommand
{
    private readonly IGameLoop _gameLoop;

    public ExitConsoleCommand( IGameLoop gameLoop )
    {
        _gameLoop = gameLoop;
    }

    public string Name => "Exit";

    public string Description => "Exits the game";

    public void Execute() => _gameLoop.RequestStop();
}