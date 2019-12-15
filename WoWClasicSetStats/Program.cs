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

        const String itemFile = @"C:\Users\StyxUT\source\repos\WoWClassicSetStats\WoWClasicSetStats\ItemList.json";
        const String spellFile = @"C:\Users\StyxUT\source\repos\WoWClassicSetStats\WoWClasicSetStats\SpellList.json";
        private static List<Item> itemList = LoadItems(File.ReadAllText(itemFile));
        private static List<Spell> spellList = LoadSpells(File.ReadAllText(spellFile));

        static void Main(string[] args)
        {
            bool quit = false;
            PrintStats();

            while (!quit)
            {
                SimulateDamage("Lightning Bolt - R4");
                SimulateDamage("Lightning Bolt - R7");
                SimulateDamage("Lightning Bolt - R8");
                SimulateDamage("Lightning Bolt - R10");

                // run again or quit
                Console.WriteLine("Press 'n' to run again, or any other key to quit.");
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                quit = (keyInfo.Key == ConsoleKey.N) ? false : true;
            }
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

        static public List<Item> LoadItems(String jsonItemList)
        {
            return JsonConvert.DeserializeObject<List<Item>>(jsonItemList);
        }

        static public List<Spell> LoadSpells(String jsonSpellList)
        {
            return JsonConvert.DeserializeObject<List<Spell>>(jsonSpellList);
        }

        // Simulates damage done
        static public void SimulateDamage(String spellName)
        {
            int damage = 0;
            int duration = 0;
            int maxHit = 0;
            int totalRegenerated = 0;
            int manaPool = CalculationHelpers.CalculateItemSetMana(itemList);
            Spell spell = CalculationHelpers.GetSpell(spellList, spellName);
            double critPercentage = CalculationHelpers.CalculateItemSetCritPercentage(itemList);
            int spellPower = CalculationHelpers.SumItemSetAttribute(itemList, "Damage");
            int spellHit = CalculationHelpers.SumItemSetAttribute(itemList, "Hit");

            for (int currentMana = manaPool; currentMana >= spell.Mana; currentMana = (currentMana - spell.Mana))
            {
                int hitDamage = CalculationHelpers.CalculateSpellDamage(spell, spellPower, critPercentage, spellHit, isBoss);
                damage += hitDamage;
                maxHit = (hitDamage > maxHit) ? hitDamage : maxHit;

                duration += Math.Max(spell.CastTime, spell.Cooldown);

                // regenerate mana
                int regeneratedMana = CalculateRegeneratedMana(duration, totalRegenerated);
                totalRegenerated += regeneratedMana;
                currentMana += regeneratedMana;
            }

            Console.WriteLine();
            Console.WriteLine($"[{spell.Name}]");
            Console.WriteLine($"Max Hit: {maxHit}");
            Console.WriteLine($"Total Damage: {damage}");
            Console.WriteLine($"Total Mana Regenerated: {totalRegenerated}");
            Console.WriteLine($"Total Duration (seconds): {duration}");
            Console.WriteLine($"DPS: {damage / duration}");
        }

        static public int CalculateRegeneratedMana(int duration, int totalRegenerated)
        {
            int MP5 = CalculationHelpers.SumItemSetAttribute(itemList, "MP5");
            int MP2 = (MP5 * 2) / 5;
            int effectiveDuration = (duration % 2 == 0) ? duration : duration - 1;

            return (effectiveDuration / 2 * MP2) - totalRegenerated;
        }

    }
}
