namespace Common
{
    public enum EthosType
    {
        None            = DataIndex.None           , // 0
        Vacillation     = DataIndex.Vacillation    , // 12010101,
        Fortitude       = DataIndex.Fortitude      , // 12010202,
        Obstinacy       = DataIndex.Obstinacy      , // 12010303,
        Cowardice       = DataIndex.Cowardice      , // 12020104,
        Valor           = DataIndex.Valor          , // 12020205,
        Recklessness    = DataIndex.Recklessness   , // 12020306,
        Rash            = DataIndex.Rash           , // 12030107,
        Cunning         = DataIndex.Cunning        , // 12030208,
        Obsession       = DataIndex.Obsession      , // 12030309,
        Conventionality = DataIndex.Conventionality, // 12040110,
        Creativity      = DataIndex.Creativity     , // 12040211,
        Reverie         = DataIndex.Reverie        , // 12040312,
        Ignorance       = DataIndex.Ignorance      , // 12050113,
        Wisdom          = DataIndex.Wisdom         , // 12050214,
        Arrogance       = DataIndex.Arrogance      , // 12050315,
        Apathy          = DataIndex.Apathy         , // 12060116,
        Benevolence     = DataIndex.Benevolence    , // 12060217,
        Voracity        = DataIndex.Voracity       , // 12060318,
    }

    public static class EthosExtension
    {
        public static DataIndex ConvertToDataIndex(this EthosType type) => (DataIndex)type;
        public static int GetIndex(this EthosType type) => type.ConvertToDataIndex().GetIndexBar();
        public static int GetGroupIndex(this EthosType type) => type.ConvertToDataIndex().GetGroupBar();
        public static int GetSubCategoryIndex(this EthosType type) => type.ConvertToDataIndex().GetSubCategoryBar();
        public static int GetCategoryIndex(this EthosType type) => type.ConvertToDataIndex().GetCategoryBar();
        public static ThemeType GetTheme(this EthosType type)
        {
            var ethosGroupIndex = type.GetSubCategoryIndex();
            var themeType = ethosGroupIndex.FindIndex<ThemeType>();

            return themeType;
        }
        
        
        public static bool IsVice(this EthosType type) => GetGroupIndex(type)       != 2;
        public static bool IsVirtue(this EthosType type) => GetGroupIndex(type)     == 2;
        public static bool IsDeficiency(this EthosType type) => GetGroupIndex(type) == 1;
        public static bool IsExcess(this EthosType type) => GetGroupIndex(type)     == 3;
        
    }
}
