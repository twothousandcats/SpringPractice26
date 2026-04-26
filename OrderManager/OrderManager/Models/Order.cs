namespace OrderManager.Models;

public sealed class Order
{
    public string ProductName { get; }
    public int Quantity { get; }
    public string CustomerName { get; }
    public string Address { get; }

    internal Order( string productName, int quantity, string customerName, string address )
    {
        ProductName = productName;
        Quantity = quantity;
        CustomerName = customerName;
        Address = address;
    }
}