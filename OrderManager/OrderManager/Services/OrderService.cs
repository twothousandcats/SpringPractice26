using System;
using OrderManager.Models;
using OrderManager.UI;

namespace OrderManager.Services;

public sealed class OrderService
{
    private const int _deliveryDaysFromNow = 3;
    private readonly IUserInterface _ui;

    public OrderService(IUserInterface ui)
    {
        _ui = ui ?? throw new ArgumentNullException(nameof(ui));
    }

    public void PlaceOrder()
    {
        Order order = CollectOrder();
        if (ConfirmOrder(order))
        {
            AnnounceSuccess(order);
        }
        else
        {
            _ui.WriteLine("Заказ отменен!");
        }
    }

    private Order CollectOrder()
    {
        OrderBuilder builder = new ();
        builder.WithProductName(PromptForNonEmpty("Введите название товара: "));
        builder.WithQuantity(PromptForPositiveInt("Введите количество товара: "));
        builder.WithCustomerName(PromptForNonEmpty("Введите ваше имя: "));
        builder.WithAddress(PromptForNonEmpty("Введите ваш адрес: "));
        return builder.Build();
    }

    private bool ConfirmOrder(Order order)
    {
        _ui.WriteLine(
            $"Здравствуйте, {order.CustomerName}, вы заказали " +
            $"{order.Quantity} {order.ProductName} на адрес {order.Address}, все верно?"
        );

        return _ui.AskYesNoQuestion("Ответ (y/n): ");
    }

    private void AnnounceSuccess(Order order)
    {
        DateTime deliveryDate = DateTime.Today.AddDays(_deliveryDaysFromNow);
        _ui.WriteLine(
            $"{order.CustomerName}! Ваш заказ {order.ProductName} в количестве " +
            $"{order.Quantity} оформлен! Ожидайте доставку по адресу {order.Address} " +
            $"к {deliveryDate:dd.MM.yyyy}"
        );
    }

    private string PromptForNonEmpty(string prompt)
    {
        while (true)
        {
            string input = _ui.ReadLine(prompt);
            if (!string.IsNullOrWhiteSpace(input))
            {
                return input.Trim();
            }

            _ui.WriteLine("Значение не может быть пустым! Попробуйте еще раз.");
        }
    }

    private int PromptForPositiveInt(string prompt)
    {
        while (true)
        {
            string input = _ui.ReadLine(prompt);
            if (int.TryParse(input, out int value) && value > 0)
            {
                return value;
            }

            _ui.WriteLine("Введите положительное целое число.");
        }
    }
}
