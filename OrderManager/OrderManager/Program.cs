using System;
using OrderManager.Services;
using OrderManager.UI;

namespace OrderManager;

internal static class Program
{
    private static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        ConsoleUserInterface ui = new ConsoleUserInterface();
        OrderService service = new( ui );
        service.PlaceOrder();
    }
}