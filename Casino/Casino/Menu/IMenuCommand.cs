namespace Casino.Menu;

public interface IMenuCommand
{
    string Title
    {
        get;
    }
    
    void Execute();
}