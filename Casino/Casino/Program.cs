using System.Globalization;
using Casino.Domain;
using Casino.Infrastructure;
using Casino.Menu;
using Casino.Menu.Commands;

namespace Casino;

public class Program
{
    private const int MinBalance = 0;
    private const int DefaultMultiplier = 3;

    public static void Main()
    {
        IInputOutput io = new ConsoleInputOutput();
        Banner.Print( io );

        decimal balance = ReadInitialBalance( io );
        IRandomGenerator rng = new RandomGenerator();
        Game game = new Game( balance, DefaultMultiplier, rng );

        MenuRunner menuRunner = new MenuRunner( io );
        menuRunner.Add( new StartCommand( io, game ) );
        menuRunner.Add( new ShowBalanceCommand( io, game ) );
        menuRunner.Add( new PlayRoundCommand( io, game ) );
        menuRunner.Add( new ExitCommand( io, menuRunner.RequestExit ) );

        menuRunner.Run();
    }

    private static decimal ReadInitialBalance( IInputOutput io )
    {
        while ( true )
        {
            io.WriteLine( "Введите начальный баланс: " );
            string? input = io.ReadLine();
            if ( decimal.TryParse( input, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal balance ) &&
                 balance > MinBalance )
            {
                return balance;
            }

            io.WriteLine( "Некорректное значение! Введите положительный баланс: " );
        }
    }
}