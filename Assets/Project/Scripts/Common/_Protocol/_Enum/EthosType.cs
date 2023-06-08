using UnityEngine;

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

        public static EthosType GetSameThemeDeficiency(this EthosType type)
        {
            if (type.IsDeficiency()) return type;
            if (type.IsVirtue()) return type.PrevExceptNone();

            return type.IsExcess() ? type.PrevExceptNone().PrevExceptNone() : EthosType.None;
        }
        public static EthosType GetSameThemeVirtue(this EthosType type)
        {
            if (type.IsVirtue()) return type;
            if (type.IsDeficiency()) return type.NextExceptNone();

            return type.IsExcess() ? type.PrevExceptNone() : EthosType.None;
        }
        public static EthosType GetSameThemeExcess(this EthosType type)
        {
            if (type.IsExcess()) return type;
            if (type.IsDeficiency()) return type.NextExceptNone().NextExceptNone();

            return type.IsVirtue() ? type.NextExceptNone() : EthosType.None;
        }

        public static EthosType GetNextVice(this EthosType type)
        {
            while (true)
            {
                var nextEthos = type.NextExceptNone();

                if (nextEthos.IsVice()) return nextEthos;
                type = nextEthos;
            }
        }
        public static EthosType GetPrevVice(this EthosType type)
        {
            while (true)
            {
                var prevEthos = type.PrevExceptNone();

                if (prevEthos.IsVice()) return prevEthos;
                type = prevEthos;
            }
        }

        public static ViceMaterialType ConvertToViceMaterial(this EthosType viceType)
        {
            switch (viceType)
            {
                case EthosType.Vacillation:     return ViceMaterialType.ShardOfVacillation;
                case EthosType.Obstinacy:       return ViceMaterialType.StoneOfObstinacy;
                case EthosType.Cowardice:       return ViceMaterialType.ShardOfCowardice;
                case EthosType.Recklessness:    return ViceMaterialType.StoneOfRecklessness;
                case EthosType.Rash:            return ViceMaterialType.ShardOfRash;
                case EthosType.Obsession:       return ViceMaterialType.StoneOfObsession;
                case EthosType.Conventionality: return ViceMaterialType.ShardOfConventionality;
                case EthosType.Reverie:         return ViceMaterialType.StoneOfReverie;
                case EthosType.Ignorance:       return ViceMaterialType.ShardOfIgnorance;
                case EthosType.Arrogance:       return ViceMaterialType.StoneOfArrogance;
                case EthosType.Apathy:          return ViceMaterialType.ShardOfApathy;
                case EthosType.Voracity:        return ViceMaterialType.StoneOfVoracity;
                default:
                {
                    UnityEngine.Debug.LogError($"Only Vice Type can Convert To ViceMaterialType. Input:{viceType}");
                    return ViceMaterialType.None;
                }
            }
        }


        public static bool IsVice(this EthosType type) => GetGroupIndex(type)       != 2;
        public static bool IsVirtue(this EthosType type) => GetGroupIndex(type)     == 2;
        public static bool IsDeficiency(this EthosType type) => GetGroupIndex(type) == 1;
        public static bool IsExcess(this EthosType type) => GetGroupIndex(type)     == 3;

        public static EthosType ConvertToEthosType(this DataIndex dataIndex)
        {
            if (dataIndex.GetCategory() != DataIndex.Villain)
            {
                Debug.LogWarning($"Only VillainCode can be converted. Input:{dataIndex.ToString()}");
                return EthosType.None;
            }

            var groupIndex = dataIndex.GetGroupBar();

            return groupIndex.FindIndex<EthosType>();
        }
    }
}
