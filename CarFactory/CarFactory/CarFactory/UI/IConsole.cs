namespace CarFactory.UI;

public interface IConsole
{
    string? ReadLine();
    void WriteLine( string message );
}