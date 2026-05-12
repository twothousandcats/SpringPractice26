namespace Fighters.Commands;

public class CommandRegistry
{
    private readonly Dictionary<string, IConsoleCommand> _commandByNames = new( StringComparer.OrdinalIgnoreCase );

    public IReadOnlyCollection<IConsoleCommand> All => _commandByNames.Values;

    public void Register( IConsoleCommand consoleCommand )
    {
        _commandByNames[ consoleCommand.Name ] = consoleCommand;
    }

    public bool TryGet( string name, out IConsoleCommand consoleCommand )
    {
        return _commandByNames.TryGetValue( name, out consoleCommand! );
    }
}