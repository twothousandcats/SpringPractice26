namespace Casino.Menu.Commands;

public interface IBetReader
{
    decimal Read( decimal maxAvailable );
}