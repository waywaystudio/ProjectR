using System;
using System.Collections.Generic;

namespace Common
{
    public class CombatMultiplier
    {
        private Dictionary<string, Func<ICombatProvider, ICombatTaker, float>> Table { get; } = new();

        public void Add(string key, Func<ICombatProvider, ICombatTaker, float> multiplier)
        {
            Table.Add(key, multiplier);
        }

        public void Remove(string key) => Table.Remove(key);

        public float Multiplier(ICombatProvider provider, ICombatTaker taker)
        {
            var result = 1f;
            
            Table.Values.ForEach(multiplier => result *= multiplier.Invoke(provider, taker));

            return result;
        }
    }
}
