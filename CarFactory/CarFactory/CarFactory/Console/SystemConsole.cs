namespace CarFactory.Console;

public class SystemConsole : IConsole
{
    public string? ReadLine() => System.Console.ReadLine();
    public void WriteLine( string message ) => System.Console.WriteLine( message );
    public void Write( string message ) => System.Console.Write( message );
}