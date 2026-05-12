using Casino.Domain;

namespace Casino.Menu.Commands;

public interface IRoundResultPrinter
{
    void Print( RoundResult result );
}