using System;

namespace WoWClassicSetStats
{

    /// <summary>
    /// Summary description for Item
    /// </summary>
    public class Item
    {
        //todo: add item slot class
        // slot
        // number of slots (e.g. 2 trinkets)
        public int WoWHeadId { get; set; }
        public String Name { get; set; }

        // base stats
        public int Intelect { get; set; }
        public int Stamina { get; set; }
        public int Spirit { get; set; }
        public int Agility { get; set; }
        public int Armor { get; set; }

        // resistances
        public int FireResistance { get; set; }
        public int FrostResistance { get; set; }
        public int ShadowResistance { get; set; }

        // other
        public int Crit { get; set; }
        public int Damage { get; set; }
        public int Healing { get; set; }
        public int MP5 { get; set; }
        public int Hit { get; set; }

    }
}