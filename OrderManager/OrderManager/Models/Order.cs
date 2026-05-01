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

    internal Order(string productName, int quantity, string customerName, string address)
    {
        if (string.IsNullOrWhiteSpace(productName))
        {
            throw new ArgumentException("Product name cannot be empty. ", nameof(productName));
        }

        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), " Quantity must be positive.");
        }

        if (string.IsNullOrWhiteSpace(customerName))
        {
            throw new ArgumentException("Customer name cannot be empty. ", nameof(customerName));
        }

        if (string.IsNullOrWhiteSpace(address))
        {
            throw new ArgumentException("Address cannot be empty. ", nameof(address));
        }

        ProductName = productName;
        Quantity = quantity;
        CustomerName = customerName;
        Address = address;
        Date = DateTime.Today.AddDays(DeliveryDaysFromNow).ToString("dd.MM.yyyy");
    }
}
