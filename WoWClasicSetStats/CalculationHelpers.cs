using System;
using System.Collections.Generic;

namespace WoWClassicSetStats
{
    public static class CalculationHelpers
    {
        const int manaPerIntelect = 15;
        const double intelectPerCritPercent = 59.2;
        //TODO: mobe baseIntelect into character class
        const int baseIntelect = 85;
        const int baseManaPool = 2515;


        public static int SumItemSetAttribute(List<Item> itemList, String attribute)
        {
            int attributeTotal = 0;
            try
            {
                foreach (Item item in itemList)
                {
                    attributeTotal += (int)item.GetType().GetProperty(attribute).GetValue(item);
                }
            }
            catch(NullReferenceException)
            {
                // expected when item does not have a value for the specified attribute
            }

            return attributeTotal;
        }

        public static int CalculateItemSetMana(List<Item> itemList)
        {
            return ((SumItemSetAttribute(itemList, "Intelect")) * manaPerIntelect) + baseManaPool;
        }

        public static double CalculateItemSetCritPercentage(List<Item> itemList)
        {
            return ((SumItemSetAttribute(itemList, "Intelect") + baseIntelect) / intelectPerCritPercent) + SumItemSetAttribute(itemList, "Crit");
        }

        // Generate a random number between two numbers  
        public static int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        public static Spell GetSpell(List<Spell> spellList, String spellName)
        {
            return spellList.Find(s => s.Name == spellName);
        }

        /// <summary>
        /// Calculates the damage/healing of a spell cast
        /// </summary>
        /// <param name="spell">instance of a spell</param>
        /// <param name="spellPower">item effect on healing or damage</param>
        /// <param name="critPercentage">chance for a spell to cirit</param>
        /// <returns>damage done</returns>
        public static int CalculateSpellDamage(Spell spell, int spellPower, double critPercentage, int spellHit = 0, bool isBoss = true)
        {
            /*
            Against level 60 targets, you need a total of 3 % Spell Hit Chance to not miss a Spell.
            Against level 63(or Boss level) targets, you need a total of 16 % Spell Hit Chance to not miss a Spell.
            */
            const int resistRateBoss = 16; // considered lvl 63
            const int resistRate60 = 3;
            int resistRate = (isBoss == true) ? resistRateBoss : resistRate60;
           
            // 0 damage if target resists
            if (RandomNumber(1, 100) <= (resistRate - spellHit))
                return 0;            

            // random damage between low & high range
            int baseDamage = RandomNumber(spell.Low, spell.High);

            // apply coeficient to spellPower damage 
            double spellPowerDamage = spellPower * (spell.Coeficient/100);

            // round and convert damage to integer
            int damage = baseDamage + (int)Math.Round(spellPowerDamage, 0);

            // check for crit and double damage
            if (RandomNumber(1, 100) <= critPercentage)
                damage += damage;

            return damage;
        }

    }
}
