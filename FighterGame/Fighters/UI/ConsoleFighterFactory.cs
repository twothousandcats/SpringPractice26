using Fighters.Models;
using Fighters.Models.Armors;
using Fighters.Models.Classes;
using Fighters.Models.Fighters;
using Fighters.Models.Races;
using Fighters.Models.Weapons;

namespace Fighters.UI
{
    public class ConsoleFighterFactory : IFighterFactory
    {
        private readonly IConsole _console;
        private readonly IReadOnlyList<Func<IArmor>> _armors;
        private readonly IReadOnlyList<Func<IFighterClass>> _classes;
        private readonly IReadOnlyList<Func<IRace>> _races;
        private readonly IReadOnlyList<Func<IWeapon>> _weapons;

        public ConsoleFighterFactory(
            IConsole console,
            IReadOnlyList<Func<IArmor>> armors,
            IReadOnlyList<Func<IFighterClass>> classes,
            IReadOnlyList<Func<IRace>> races,
            IReadOnlyList<Func<IWeapon>> weapons
        )
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _races = races ?? throw new ArgumentNullException(nameof(races));
            _classes = classes ?? throw new ArgumentNullException(nameof(classes));
            _weapons = weapons ?? throw new ArgumentNullException(nameof(weapons));
            _armors = armors ?? throw new ArgumentNullException(nameof(armors));
        }

        public IFighter Create()
        {
            string name = ReadName();
            IRace race = ReadFromList("Choose race: ", _races);
            IFighterClass classes = ReadFromList("Choose class: ", _classes);
            IWeapon weapons = ReadFromList("Choose weapon: ", _weapons);
            IArmor armor = ReadFromList("Choose armor: ", _armors);

            return new Fighter(name, race, classes, weapons, armor);
        }

        private string ReadName()
        {
            while (true)
            {
                _console.WriteLine("Enter fighter name:");
                string? name = _console.ReadLine();
                if (!string.IsNullOrWhiteSpace(name))
                {
                    return name;
                }

                _console.WriteLine("Name cant be empty!");
            }
        }

        private T ReadFromList<T>(string title, IReadOnlyList<Func<T>> options) where T : IName
        {
            T[] samples = options.Select(f => f()).ToArray();
            while (true)
            {
                _console.WriteLine(title);
                for (int i = 0; i < samples.Length; i++)
                {
                    _console.WriteLine($"{i + 1} - {samples[i].Name}");
                }

                string? input = _console.ReadLine();
                if (int.TryParse(input, out int choice) && choice >= 1 && choice <= options.Count)
                {
                    return options[choice - 1]();
                }

                _console.WriteLine("Invalid option. Try again.");
            }
        }
    }
}
