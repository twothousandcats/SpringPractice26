using Casino.Infrastructure;

namespace Casino.Menu;

public static class Banner
{
    private static readonly string[] Heading = 
    {
        " #####    ###    ####   #   #   #   ### ",
        "#        #   #  #           ##  #  #   #",
        "#        #####   ###    #   # # #  #   #",
        "#        #   #      #   #   #  ##  #   #",
        " #####   #   #  ####    #   #   #   ### ",
    };

    public static void Print( IInputOutput io )
    {
        ArgumentNullException.ThrowIfNull( io );
        foreach ( string headingLine in Heading )
        {
            io.WriteLine( headingLine );
        }

        io.WriteLine( string.Empty );
    }
}