using Fighters.Models.Fighters;
using Fighters.UI;

namespace Fighters
{
    public class Program
    {
        public static void Main(string[] args)
        {
            List<IFighter> arena = new List<IFighter>();
            GameManager gameManager = new GameManager();

            Console.WriteLine("Commands: add-fighter, play, quit");

            while (true)
            {
                Console.Write("> ");
                string? command = Console.ReadLine()?.Trim();

                if (command is null or "quit" or "exit")
                {
                    return;
                }

                if (command == "add-fighter")
                {
                    try
                    {
                        IFighter fighter = ConsoleFighterFactory.CreateFromConsole();
                        arena.Add(fighter);
                        Console.WriteLine($"Added fighter {fighter.Name}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to add fighter: {ex.Message}");
                    }
                }
                else if (command == "play")
                {
                    if (arena.Count < 2)
                    {
                        Console.WriteLine("Need at least 2 fighters");
                        continue;
                    }

                    gameManager.Play(arena);
                    arena.Clear();
                }
                else
                {
                    Console.WriteLine($"Invalid command: {command}. Use add-fighter, play, quit");
                }
            }
        }
    }
}
