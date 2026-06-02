namespace Fighters.Models.Classes;

public class Knight : IFighterClass
{
    public string Name => "Knight";

    public int Damage => 5;

    public int Health => 50;
}