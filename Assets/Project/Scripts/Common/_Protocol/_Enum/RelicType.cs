namespace Common
{
    public enum RelicType
    {
        None     = 0,
        Vowed    = DataIndex.Vowed,
        Valorous = DataIndex.Valorous,
        Veterans = DataIndex.Veterans,
        Vital    = DataIndex.Vital,
        Verdicts = DataIndex.Verdicts,
        Votive   = DataIndex.Votive,
    }

    public static class RelicExtension
    {
        public static DataIndex ConvertToDatIndex(this RelicType relicType) => (DataIndex)relicType;
        public static EthosType CovertToVirtue(this RelicType relicType)
        {
            // ex.31000006 to 6
            var viceIndex = ((int)relicType).GetNumberOfFromToDestDigits(1, 2);
            
            // 19 * 100 + 6 = 1906
            var ethosIndex = (EthosType)((int)DataIndex.Ethos * 100 + viceIndex);

            return ethosIndex;
        }
    }
}
