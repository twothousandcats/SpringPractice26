using Casino.Infrastructure;

namespace Casino.Menu;

public sealed class Menu
{
    private readonly IInputOutput _io;

    // commands
    //private bool _shouldExit;

    public Menu(IInputOutput io)
    {
        ArgumentNullException.ThrowIfNull(io);
        _io = io;
    }

    // public void Add(command)
    // {
    //     
    // }
    //public void RequestExit() => _shouldExit = true;

    // public void Run()
    // {
    //     
    // }

    // public void PrintOptions()
    // {
    //     
    // }

    // private int? ReadChoice()
    // {
    //     
    // }
}