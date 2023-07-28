using UnityEngine;

namespace Common.Currencies
{
    public class ExperienceReward : IReward
    {
        private Sprite icon;
        
        public int Amount;
        public int Grade => 0;
        public string Title => "Experience";
        public string Description => "Party Level Source";
        public Sprite Icon => icon ??= Database.RuneSpriteData.Get(DataIndex.Experience);
        
        public ExperienceReward(int amount) => Amount = amount;

        public static ExperienceReward CreateInstance(int amount)
        {
            return new ExperienceReward(amount);
        }
    }
}
