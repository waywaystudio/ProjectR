namespace Common
{
    public enum RewardRuneType
    {
        None = 0,
        Gold = DataIndex.Gold,
        Experience = DataIndex.Experience,
        Skill = 100,
        // Stat,
        // EquipmentRune,
        // EthosRune,
    }

    public enum TaskRuneType
    {
        None = 0,
        NoDamage,
        Victory,
        TimeLimit,
        SkillCount,
        SkillDamage,
    }
    
    public static class RuneTypeExtension
    {
        public static DataIndex ToDataIndex(this RewardRuneType runeType)
        {
            return (DataIndex)runeType;
        }
    }
}
