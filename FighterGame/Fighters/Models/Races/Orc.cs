namespace Fighters.Models.Races
{
    public class Orc : IRace
    {
        public string Name => "Orc";
        public int Damage => 5;
        public int Health => 120;
        public int Armor => 1;
        public int Initiative => 4;
    }
}
