using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using WoWClassicSetStats;

namespace XUnitTestProject
{
    public class ListFixture : IDisposable
    {
        const String itemFile = @"C:\Users\StyxUT\source\repos\WoWClassicSetStats\WoWClasicSetStats\TestItemList.json";
        const String spellFile = @"C:\Users\StyxUT\source\repos\WoWClassicSetStats\WoWClasicSetStats\TestSpellList.json";

        public List<Item> ItemList { get; private set; }
        public List<Spell> SpellList { get; private set; }

        public ListFixture()
        {
            ItemList = Program.LoadItems(File.ReadAllText(itemFile));
            SpellList = Program.LoadSpells(File.ReadAllText(spellFile));
        }

        public void Dispose()
        {
        }

    }

    public class CalculationHelperTests
    {
        ListFixture listFixture = new ListFixture();

        [Theory]
        [InlineData(125, "Intelect")]
        [InlineData(63, "Spirit")]
        [InlineData(5, "MP5")]
        [InlineData(5, "Crit")]
        [InlineData(280, "Damage")]
        [InlineData(1, "Hit")]
        [InlineData(0, "fakeAttribute")]
        public void TestCalculateItemSetAttribute(int expectedValue, String attribute)
        {
            Assert.Equal(expectedValue, CalculationHelpers.SumItemSetAttribute(listFixture.ItemList, attribute));
        }

        [Fact]
        public void TestCalculateItemSetMana()
        {
            Assert.Equal(4390, CalculationHelpers.CalculateItemSetMana(listFixture.ItemList));
        }

        [Fact]
        public void TestCalculateItemSetCritPercentage()
        {
            // round to 9 decimal digits
            Assert.Equal(8.547297297, Math.Round(CalculationHelpers.CalculateItemSetCritPercentage(listFixture.ItemList), 9));
        }

        [Theory]
        [InlineData("Lightning Bolt - R10")]
        [InlineData("Chain Lightning - R4")]
        public void TestGetSpell(String spellName)
        {
            Spell spell = CalculationHelpers.GetSpell(listFixture.SpellList, spellName);
            Assert.Equal(spellName, spell.Name);
        }
    }
}
