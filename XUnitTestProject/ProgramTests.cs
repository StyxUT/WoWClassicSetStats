using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using WoWClassicSetStats;

namespace XUnitTestProject
{
    public class ProgramTests
    {
        [Theory]
        [InlineData(90, 100, 10)]
        public void CalculateRegeneratedMana(int expectedValue, int duration, int totalRegenerated)
        {
            Assert.Equal(expectedValue, Program.CalculateRegeneratedMana(duration, totalRegenerated));
        }
    }
}
