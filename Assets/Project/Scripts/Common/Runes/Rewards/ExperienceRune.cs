using System;

namespace Common.Runes.Rewards
{
    [Serializable]
    public class ExperienceRune : RewardRune
    {
        public ExperienceRune(int experience) => Experience = experience;
        
        public int Experience;
    }
}
