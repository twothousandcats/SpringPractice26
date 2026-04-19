using Casino.Infrastructure;

namespace Casino.Menu;

public static class Banner
{
    private static readonly string[] Headings = 
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
        foreach (var heading in Headings)
        {
            io.WriteLine(heading);
        }
        io.WriteLine(string.Empty);
    }
}