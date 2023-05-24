namespace Common
{
    public enum RelicType
    {
        None = 0,
        Vow = DataIndex.Fortitude,
        Valor = DataIndex.Valor,
        Veterans = DataIndex.Cunning,
        Vital = DataIndex.Creativity,
        Verdicts = DataIndex.Wisdom,
        Votive = DataIndex.Benevolence,
    }

    public static class RelicExtension
    {
        public static DataIndex ConvertToDatIndex(this   RelicType relicType) => (DataIndex)relicType;
        public static EthosType ConvertToVirtues(this    RelicType relicType) => (EthosType)relicType;
        public static EthosType ConvertToDeficiency(this RelicType relicType) => (EthosType)(relicType - 1);
        public static EthosType ConvertToExcess(this RelicType relicType) => (EthosType)(relicType + 1);
    }
}
