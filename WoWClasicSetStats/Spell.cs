using System;

namespace WoWClassicSetStats
{
    /// <summary>
    /// Spell class
    /// </summary>
    public class Spell
    {
        /// <summary>
        /// Spell name with rank. e.g. Lightningbolt - R10
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// WoWHead spell id
        /// </summary>
        public int WoWHeadId { get; set; }

        /// <summary>
        /// Spell rank
        /// </summary>
        public int Rank { get; set; }

        /// <summary>
        /// Low end of spell damage/healing effect
        /// </summary>
        public int Low { get; set; }
        
        /// <summary>
        /// High end of spell damage/healing effect
        /// </summary>
        public int High { get; set; }
        
        /// <summary>
        /// Mana cost to cast
        /// </summary>
        public int Mana { get; set; }
        
        /// <summary>
        /// Casting duration
        /// </summary>
        public int CastTime { get; set; }
        
        /// <summary>
        /// Maximum number of targets affected
        /// </summary>
        public int Targets { get; set; }

        /// <summary>
        /// Percent of effect reducted with each subsequent affected target
        /// </summary>
        public int Reduction { get; set; }
        
        /// <summary>
        /// Number of seconds before spell can be cast again
        /// </summary>
        public int Cooldown { get; set; }

        /// <summary>
        /// Spell effect coeficient (percent of bonus effect)
        /// </summary>
        public double Coeficient { get; set; }
    }
}