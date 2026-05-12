using Fighters.UI;

namespace Fighters.Tests.Helpers;

public class FakeConsole : IConsole
{
    private readonly Queue<string?> _input;

    public List<string> Output { get; } = [ ];

    public FakeConsole( params string?[] input )
    {
        _input = new Queue<string?>( input );
    }

    public string? ReadLine() => _input.Count > 0 ? _input.Dequeue() : null;
    public void WriteLine( string message ) => Output.Add( message );
    public void Write( string message ) => Output.Add( message );
}