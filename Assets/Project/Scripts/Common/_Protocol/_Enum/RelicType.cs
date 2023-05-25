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
        public static DataIndex ConvertToDatIndex(this   RelicType relicType) => (DataIndex)relicType;
    }
}
