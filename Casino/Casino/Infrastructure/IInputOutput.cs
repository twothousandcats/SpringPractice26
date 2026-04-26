namespace Casino.Infrastructure;

public interface IInputOutput
{
    void WriteLine( string message );
    void Write( string message );
    string? ReadLine();
}