using System;
using OrderManager.Models;
using OrderManager.UI;

namespace OrderManager.Controllers
{
    public sealed class OrderController
    {
        private readonly IUserInterface _ui;

        public OrderController(IUserInterface ui)
        {
            _ui = ui;
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
            string productName = PromptForNonEmpty("Введите название товара: ");
            int quantity = PromptForPositive("Введите количество товара: ");
            string customerName = PromptForNonEmpty("Введите ваше имя: ");
            string address = PromptForNonEmpty("Введите ваш адрес: ");

            return new Order(productName, quantity, customerName, address);
        }

        private bool ConfirmOrder(Order order)
        {
            _ui.WriteLine(
                $"Здравствуйте, {order.CustomerName}, вы заказали " +
                $"{order.Quantity} {order.ProductName} на адрес {order.Address}, все верно?"
            );

            return _ui.ObtainConsent("Ответ (y/n): ");
        }

        private void AnnounceSuccess(Order order)
        {
            _ui.WriteLine(
                $"{order.CustomerName}! Ваш заказ {order.ProductName} в количестве " +
                $"{order.Quantity} оформлен! Ожидайте доставку по адресу {order.Address} " +
                $"к {order.Date}"
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

        private int PromptForPositive(string prompt)
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
}
