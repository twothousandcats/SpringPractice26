using System;
using System.Globalization;
using Casino.Domain;
using Casino.Infrastructure;

public class Program
{
    private const int DefaultMultiplicator = 3;

    public static void Main()
    {
        IInputOutput io = new ConsoleInputOutput();
        // io.Write("Hello, World!");
        decimal balance = ReadInitialBalance(io);
        IRandomGenerator rng = new RandomGenerator();
        Game game = new Game(balance, DefaultMultiplicator, rng);
    }

    private static decimal ReadInitialBalance(IInputOutput io)
    {
        while (true)
        {
            io.WriteLine("Введите начальный баланс: ");
            string? input = io.ReadLine();
            if (decimal.TryParse(input, NumberStyles.Number, CultureInfo.InvariantCulture, out var balance) &&
                balance >= 0)
            {
                return balance;
            }
            
            io.WriteLine("Некорректное значение! Введите положительный баланс: ");
        }
    }
}