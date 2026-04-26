using Casino.Infrastructure;

namespace Casino.Menu;

public sealed class Menu
{
    private readonly IInputOutput _io;
    private readonly List<IMenuCommand> _commands;
    private bool _shouldExit;

    public Menu( IInputOutput io )
    {
        ArgumentNullException.ThrowIfNull( io );
        _io = io;
        _commands = new List<IMenuCommand>();
    }

    public void RequestExit() => _shouldExit = true;

    public void Add( IMenuCommand command )
    {
        ArgumentNullException.ThrowIfNull( command );
        _commands.Add( command );
    }

    public void Run()
    {
        if ( _commands.Count == 0 )
        {
            throw new InvalidOperationException( "В меню нет комманд!" );
        }

        while ( !_shouldExit )
        {
            PrintOptions();
            int? choice = ReadChoice();
            if ( choice is null )
            {
                _io.WriteLine( "Некорректный ввод. Попробоуйте снова." );
                continue;
            }

            _commands[ choice.Value - 1 ].Execute();
        }
    }

    private void PrintOptions()
    {
        _io.WriteLine( string.Empty );
        _io.WriteLine( "Меню" );
        for ( int i = 0; i < _commands.Count; i++ )
        {
            _io.WriteLine( $"{i + 1}. {_commands[ i ].Title}" );
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

        if ( choice < 1 || choice > _commands.Count )
        {
            return null;
        }

        return choice;
    }
}