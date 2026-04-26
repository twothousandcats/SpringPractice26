using Casino.Infrastructure;

namespace Casino.Menu;

public static class Banner
{
    private static readonly string[] _heading = 
    {
        " #####    ###    ####   #   #   #   ###  ",
        "#        #   #  #           ##  #  #   # ",
        "#        #####   ###    #   # # #  #   # ",
        "#        #   #      #   #   #  ##  #   # ",
        " #####   #   #  ####    #   #   #   ###  ",
    };

    public static void Print(IInputOutput io)
    {
        ArgumentNullException.ThrowIfNull(io);
        foreach (string heading in _heading)
        {
            io.WriteLine(heading);
        }
        io.WriteLine(string.Empty);
    }
}