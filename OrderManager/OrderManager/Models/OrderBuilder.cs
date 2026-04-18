using System;

namespace OrderManager.Models;

public sealed class OrderBuilder
{
    private string? _productName;
    private int? _quantity;
    private string? _customerName;
    private string? _address;

    public OrderBuilder WithProductName(string productName)
    {
        _productName = RequireNonEmpty(productName, nameof(productName));
        return this;
    }

    public OrderBuilder WithQuantity(int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(quantity),
                quantity,
                "Quantity must be positive"
            );
        }

        _quantity = quantity;
        return this;
    }

    public OrderBuilder WithCustomerName(string customerName)
    {
        _customerName = RequireNonEmpty(customerName, nameof(customerName));
        return this;
    }

    public OrderBuilder WithAddress(string address)
    {
        _address = RequireNonEmpty(address, nameof(address));
        return this;
    }

    public Order Build()
    {
        if (_productName is null || _quantity is null || _customerName is null || _address is null)
        {
            throw new InvalidOperationException("All order fields must be set before building.");
        }

        return new Order(_productName, _quantity.Value, _customerName, _address);
    }

    private static string RequireNonEmpty(string value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value must not be empty!", parameterName);
        }

        return value.Trim();
    }
}
