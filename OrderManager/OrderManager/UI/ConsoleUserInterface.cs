using System;

namespace OrderManager.UI;

public sealed class ConsoleUserInterface : IUserInterface
{
    public string ReadLine( string prompt )
    {
        Console.Write( prompt );
        return Console.ReadLine() ?? string.Empty;
    }

    public void WriteLine( string message )
    {
        Console.WriteLine( message );
    }

    public bool AskYesNoQuestion( string prompt )
    {
        while ( true )
        {
            var answer = ReadLine( prompt ).Trim().ToLowerInvariant();
            switch ( answer )
            {
                case "y":
                case "yes":
                case "д":
                case "да":
                    return true;
                case "n":
                case "no":
                case "н":
                case "нет":
                    return false;
                default:
                    WriteLine( "Пожалуйста, введите 'y' (да) или 'n' (нет)." );
                    break;
            }
        }
    }
}