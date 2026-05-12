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
        IRandomGenerator rng = new RandomGenerator( new Random() );
        Game game = new Game( balance, DefaultMultiplier, rng );

        io.WriteLine( $"Игра началась. Ваш начальный баланс: {game.Balance:0.##}" );

        IBetReader betReader = new ConsoleBetReader( io );
        IRoundResultPrinter roundResultPrinter = new ConsoleRoundResultPrinter( io );

        MenuRunner menuRunner = new MenuRunner( io );
        menuRunner.Add(
            new MenuItem(
                "Показать баланс",
                new ShowBalanceCommand( io, game )
            )
        );

        menuRunner.Add(
            new MenuItem(
                "Сыграть раунд",
                new PlayRoundCommand(
                    io,
                    game,
                    betReader,
                    roundResultPrinter
                )
            )
        );

        menuRunner.Add(
            new MenuItem(
                "Выйти",
                new ExitCommand( io, menuRunner.RequestExit )
            )
        );

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