namespace Common
{
    public enum ThemeType
    {
        None = 0,
        Vow = DataIndex.Vow, // 11000001
        Valiance = DataIndex.Valiance, // 11000002
        Veteran = DataIndex.Veteran, // 11000003
        Vision = DataIndex.Vision, // 11000004
        Verity = DataIndex.Verity, // 11000005
        Veneration = DataIndex.Veneration, // 11000006
    }

    public static class ThemeExtension
    {
        public static DataIndex ConvertToDataIndex(this ThemeType type) => (DataIndex)type;
        public static int GetThemeIndex(this ThemeType type) => type.ConvertToDataIndex().GetIndexBar();
        public static ThemeType GetNextTheme(this ThemeType type) => type.NextExceptNone();
    }
}
