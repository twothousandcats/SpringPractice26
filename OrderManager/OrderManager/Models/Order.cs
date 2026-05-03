using System;

namespace OrderManager.Models;

public sealed class Order
{

    public string ProductName { get; }
    public int Quantity { get; }
    public string CustomerName { get; }
    public string Address { get; }
    public DateTime Date { get; }

    internal Order( string productName, int quantity, string customerName, string address, DateTime date )
    {
        ProductName = productName;
        Quantity = quantity;
        CustomerName = customerName;
        Address = address;
        Date = date;
    }
}