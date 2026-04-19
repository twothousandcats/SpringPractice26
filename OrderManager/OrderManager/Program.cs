using System;
using OrderManager.Services;
using OrderManager.UI;

namespace OrderManager;

internal static class Program
{
    private static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        var ui = new ConsoleUserInterface();
        var service = new OrderService(ui);
        service.PlaceOrder();
    }
}
