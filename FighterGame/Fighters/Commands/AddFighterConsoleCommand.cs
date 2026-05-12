using Fighters.Models.Fighters;
using Fighters.UI;

namespace Fighters.Commands;

public class AddFighterConsoleCommand : IConsoleCommand
{
    private readonly FighterRoster _fighterRoster;

    private readonly IFighterFactory _factory;

    private readonly IConsole _console;

    public AddFighterConsoleCommand( FighterRoster fighterRoster, IFighterFactory factory, IConsole console )
    {
        _fighterRoster = fighterRoster;
        _factory = factory;
        _console = console;
    }

    public string Name => "Add";

    public string Description => "Adds a fighter to the arena";

    public void Execute()
    {
        IFighter fighter = _factory.Create();
        _fighterRoster.Add( fighter );
        _console.WriteLine( $"Fighter {fighter.Name} added to the arena" );
    }
}