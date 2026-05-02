using Fighters.Models.Armors;
using Fighters.Models.Classes;
using Fighters.Models.Races;
using Fighters.Models.Weapons;

namespace Fighters.UI;

public static class FighterCatalog
{
    public static IReadOnlyList<Func<IArmor>> Armors { get; } =
    [
        () => new NoArmor(),
        () => new SimpleClothes(),
        () => new LeatherArmor(),
        () => new ChainArmor(),
        () => new PlateArmor(),
    ];

    public static IReadOnlyList<Func<IFighterClass>> Classes { get; } =
    [
        () => new Knight(),
        () => new Mage(),
        () => new Mercenary(),
    ];

    public static IReadOnlyList<Func<IRace>> Races { get; } =
    [
        () => new Human(),
        () => new Elf(),
        () => new Orc(),
        () => new Dwarf(),
    ];

    public static IReadOnlyList<Func<IWeapon>> Weapons { get; } =
    [
        () => new Axe(),
        () => new Bow(),
        () => new Fists(),
        () => new Staff(),
        () => new Sword(),
    ];
}