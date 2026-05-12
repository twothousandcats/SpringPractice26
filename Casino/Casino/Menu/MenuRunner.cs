using Casino.Infrastructure;
using Casino.Menu.Commands;

namespace Casino.Menu;

public sealed class MenuRunner
{
    private readonly IInputOutput _io;

    private readonly List<MenuItem> _items;

    private bool _isRunning;

    public MenuRunner( IInputOutput io )
    {
        _io = io;
        _items = new List<MenuItem>();
        _isRunning = true;
    }

    public void RequestExit() => _isRunning = false;

    public void Add( MenuItem command )
    {
        _items.Add( command );
    }

    public void Run()
    {
        if ( _items.Count == 0 )
        {
            throw new InvalidOperationException( "В меню нет команд!" );
        }

        PrintOptions();
        while ( _isRunning )
        {
            int? choice = ReadChoice();
            if ( choice is null )
            {
                _io.WriteLine( "Некорректный ввод. Попробуйте снова." );
                continue;
            }

            _items[ choice.Value - 1 ].Command.Execute();
        }
    }

    private void PrintOptions()
    {
        _io.WriteLine( string.Empty );
        _io.WriteLine( "Меню" );
        for ( int i = 0; i < _items.Count; i++ )
        {
            _io.WriteLine( $"{i + 1}. {_items[ i ].Title}" );
        }

        _io.WriteLine( "Выберите команду: " );
    }

    private int? ReadChoice()
    {
        string? input = _io.ReadLine();
        if ( !int.TryParse( input, out int choice ) )
        {
            return null;
        }

        if ( choice < 1 || choice > _items.Count )
        {
            return null;
        }

        return choice;
    }
}