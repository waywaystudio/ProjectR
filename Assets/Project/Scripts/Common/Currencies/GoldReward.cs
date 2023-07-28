using UnityEngine;

namespace Common.Currencies
{
    public class GoldReward : IReward
    {
        private Sprite icon;
        
        public int Amount;
        public int Grade => 0;
        public string Title => "Gold";
        public string Description => "Common Currency in the World";
        public Sprite Icon => icon ??= Database.RuneSpriteData.Get(DataIndex.Gold);

        public GoldReward(int amount) => Amount = amount;

        public static GoldReward CreateInstance(int amount)
        {
            return new GoldReward(amount);
        }
    }
}
