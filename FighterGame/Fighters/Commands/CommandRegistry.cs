namespace Fighters.Commands;

public class CommandRegistry
{
    private readonly Dictionary<string, IConsoleCommand> _commands = new( StringComparer.OrdinalIgnoreCase );

    public IReadOnlyCollection<IConsoleCommand> All => _commands.Values;

    public void Register( IConsoleCommand consoleCommand )
    {
        _commands[ consoleCommand.Name ] = consoleCommand;
    }

    public bool TryGet( string name, out IConsoleCommand consoleCommand )
    {
        return _commands.TryGetValue( name, out consoleCommand! );
    }
}