using System;
using OrderManager.Controllers;
using OrderManager.UI;

namespace OrderManager;

internal static class Program
{
    private static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        ConsoleUserInterface ui = new ConsoleUserInterface();
        OrderController orderController = new( ui );
        orderController.PlaceOrder();
    }
}