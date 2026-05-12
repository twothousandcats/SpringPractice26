using Casino.Domain;
using Casino.Infrastructure;

namespace Casino.Menu.Commands;

public class ConsoleRoundResultPrinter : IRoundResultPrinter
{
    private readonly IInputOutput _io;

    public ConsoleRoundResultPrinter( IInputOutput io )
    {
        _io = io;
    }

    public void Print( RoundResult result )
    {
        _io.WriteLine( $"Выпал номер: {result.RolledNumber}" );
        _io.WriteLine(
            result.IsWin
                ? $"Вы выиграли: {result.Payout:0.##}!"
                : $"Вы проиграли вашу ставку {Math.Abs( result.Payout ):0.##}!"
        );

        _io.WriteLine( $"Ваш баланс: {result.NewBalance:0.##}" );
    }
}