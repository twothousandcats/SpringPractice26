namespace Fighters.Models.Races
{
    public class Human : IRace
    {
        public string Name => "Human";
        public int Damage => 1;
        public int Health => 100;
        public int Armor => 0;
    }
}
