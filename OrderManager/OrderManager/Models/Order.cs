using System;

namespace OrderManager.Models;

public sealed class Order
{
    private const int DeliveryDaysFromNow = 3;

    public string ProductName { get; }
    public int Quantity { get; }
    public string CustomerName { get; }
    public string Address { get; }
    public string Date { get; }

    internal Order( string productName, int quantity, string customerName, string address )
    {
        ProductName = productName;
        Quantity = quantity;
        CustomerName = customerName;
        Address = address;
        Date = DateTime.Today.AddDays( DeliveryDaysFromNow ).ToString( "dd.MM.yyyy" );
    }
}