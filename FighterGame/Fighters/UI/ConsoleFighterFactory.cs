using Fighters.Models.Armors;
using Fighters.Models.Classes;
using Fighters.Models.Fighters;
using Fighters.Models.Races;
using Fighters.Models.Weapons;

namespace Fighters.UI
{
    public class ConsoleFighterFactory
    {
        public static IFighter CreateFromConsole()
        {
            string name = ReadName();
            IRace race = ReadRace();
            IFighterClass fighterClass = ReadClass();
            IWeapon weapon = ReadWeapon();
            IArmor armor = ReadArmor();
            return new Fighter(name, race, fighterClass, weapon, armor);
        }

        private static string ReadName()
        {
            while (true)
            {
                Console.WriteLine("Enter fighter name:");
                string? value = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(value))
                {
                    return value.Trim();
                }

                Console.WriteLine("Name must not be empty.");
            }
        }

        private static IRace ReadRace()
        {
            Console.WriteLine("Available fighters: 1 - Human, 2 - Elf, 3 - Orc, 4 - Dwarf");
            return ReadChoice<IRace>(new()
            {
                ["1"] = () => new Human(), ["2"] = () => new Elf(), ["3"] = () => new Orc(), ["4"] = () => new Dwarf(),
            });
        }

        private static IFighterClass ReadClass()
        {
            Console.WriteLine("Available classes: 1 - Knight, 2 - Mercenary, 3 - Mage");
            return ReadChoice<IFighterClass>(new()
            {
                ["1"] = () => new Knight(), ["2"] = () => new Mercenary(), ["3"] = () => new Mage(),
            });
        }

        private static IWeapon ReadWeapon()
        {
            Console.WriteLine("Available weapons: 1 - Fists, 2 - Sword, 3 - Bow, 4 - Axe, 5 - Staff");
            return ReadChoice<IWeapon>(new()
            {
                ["1"] = () => new Fists(),
                ["2"] = () => new Sword(),
                ["3"] = () => new Bow(),
                ["4"] = () => new Axe(),
                ["5"] = () => new Staff(),
            });
        }

        private static IArmor ReadArmor()
        {
            Console.WriteLine("Available armors: 1 - No armor, 2 - Simple clothes, 3 - Leather, 4 - Chain, 5 - Plate");
            return ReadChoice<IArmor>(new()
            {
                ["1"] = () => new NoArmor(),
                ["2"] = () => new SimpleClothes(),
                ["3"] = () => new LeatherArmor(),
                ["4"] = () => new ChainArmor(),
                ["5"] = () => new PlateArmor(),
            });
        }

        private static T ReadChoice<T>(Dictionary<string, Func<T>> options)
        {
            while (true)
            {
                string? value = Console.ReadLine()?.Trim();
                if (value is not null && options.TryGetValue(value, out Func<T>? factory))
                {
                    return factory();
                }

                Console.WriteLine("Invalid option, try again.");
            }
        }
    }
}
