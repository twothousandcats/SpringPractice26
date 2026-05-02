namespace OrderManager.UI;

public interface IUserInterface
{
    string ReadLine( string prompt );
    void WriteLine( string message );
    bool ObtainConsent( string prompt );
}