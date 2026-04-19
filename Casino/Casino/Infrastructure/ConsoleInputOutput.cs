namespace Casino.Infrastructure;

public sealed class ConsoleInputOutput : IInputOutput
{
    public void WriteLine(string message) => Console.WriteLine(message);
    public void Write(string message) => Console.Write(message);
    public string? ReadLine() => Console.ReadLine();
}