using UnityEngine;

namespace Common.Currencies
{
    public class ExperienceReward : IReward
    {
        public readonly int Amount;
        public Sprite Icon { get; }

        public int Grade => 0;
        public string Title => "Experience";
        public string Description => "Party Level Source";

        // TODO. 일단 이렇게 써보고 추후에 변경하자.
        public ExperienceReward(Sprite icon, int amount)
        {
            Icon   = icon;
            Amount = amount;
        }
    }
}
