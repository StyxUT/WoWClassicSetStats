using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Reflection;


namespace WoWClassicSetStats
{
    public class Program
    {
        const bool isBoss = true; // boss is level 63 mob, else 60

        const String itemFile = @"C:\Users\StyxUT\source\repos\WoWClasicSetStats\WoWClasicSetStats\ItemList.json";
        const String spellFile = @"C:\Users\StyxUT\source\repos\WoWClasicSetStats\WoWClasicSetStats\SpellList.json";
        private static List<Item> itemList = LoadItems(File.ReadAllText(itemFile));
        private static List<Spell> spellList = LoadSpells(File.ReadAllText(spellFile));

        static void Main(string[] args)
        {
            PrintStats();

            SimulateDamage(100000, "Lightning Bolt - R4");
            SimulateDamage(100000, "Lightning Bolt - R10");
        }

        private static void PrintStats()
        {
            // get and loop through all properties that are of type int and not "WoWHeadId"
            PropertyInfo[] attributes = typeof(Item).GetProperties();
            foreach (PropertyInfo attribute in attributes) if (attribute.PropertyType == typeof(int) && attribute.Name != "WoWHeadId")
                {
                    Console.WriteLine($"Total {attribute.Name}: {CalculationHelpers.SumItemSetAttribute(itemList, attribute.Name)}");
                }

            Console.WriteLine();
            Console.WriteLine($"Total Mana: {CalculationHelpers.CalculateItemSetMana(itemList)}");
            Console.WriteLine($"Crit Percent: {CalculationHelpers.CalculateItemSetCritPercentage(itemList)}");
        }

        static public List<Item>LoadItems(String jsonItemList)
        {
            return JsonConvert.DeserializeObject<List<Item>>(jsonItemList);
        }

        static public List<Spell>LoadSpells(String jsonSpellList)
        {
            return JsonConvert.DeserializeObject<List<Spell>>(jsonSpellList);
        }

        // Simulates damage done
        static public void SimulateDamage(int simulations, String spellName)
        {
            int damage = 0;
            int duration = 0;
            int manaPool = CalculationHelpers.CalculateItemSetMana(itemList);
            Spell spell = CalculationHelpers.GetSpell(spellList, spellName);
            double critPercentage = CalculationHelpers.CalculateItemSetCritPercentage(itemList);
            int spellPower = CalculationHelpers.SumItemSetAttribute(itemList, "Damage");
            int spellHit = CalculationHelpers.SumItemSetAttribute(itemList, "Hit");

            for (int i = 0; i < simulations; i++)
            {
                damage += CalculationHelpers.CalculateSpellDamage(spell, spellPower, critPercentage, spellHit, isBoss);
                duration += Math.Max(spell.CastTime, spell.Cooldown);
            }

            Console.WriteLine();
            Console.WriteLine($"[{spell.Name}]"); 
            Console.WriteLine($"Total Damage: {damage}");
            Console.WriteLine($"Total Duration (seconds): {duration}");
            Console.WriteLine($"DPS: {damage/duration}");
        }
    }
}
