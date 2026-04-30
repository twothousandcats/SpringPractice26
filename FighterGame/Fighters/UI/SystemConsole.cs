namespace Fighters.UI
{
    public class SystemConsole : IConsole
    {
        public string? ReadLine() => Console.ReadLine();
        public void WriteLine(string message) => Console.WriteLine(message);
    }
}
