using System;

namespace Common.Runes.Rewards
{
    [Serializable]
    public class GoldRune : RewardRune
    {
        public GoldRune(int gold) => RewardGold = gold;
        
        public int RewardGold;
    }
}
