using UnityEngine;

namespace Common.Currencies
{
    public class GoldReward : IReward
    {
        public readonly int Amount;
        public Sprite Icon { get; }
        
        public int Grade => 0;
        public string Title => "Gold";
        public string Description => "Common Currency in the World";

        // TODO. 일단 이렇게 써보고 추후에 변경하자.
        public GoldReward(Sprite icon, int amount)
        {
            Icon   = icon;
            Amount = amount;
        }
    }
}
