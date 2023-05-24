using UnityEngine;

namespace Common
{
    public enum MaterialType
    {
        None              = 0,
        VowedShard        = DataIndex.Vowedshard,
        VowedStone        = DataIndex.Vowedstone,
        VowedCrystal      = DataIndex.Vowedcrystal,
        VolorousShard     = DataIndex.Volorousshard,
        VolorousStone     = DataIndex.Volorousstone,
        VolorousCrystal   = DataIndex.Volorouscrystal,
        VeteransShard     = DataIndex.Veteransshard,
        VeteransStone     = DataIndex.Veteransstone,
        VeteransCrystal   = DataIndex.Veteranscrystal,
        VitalShard        = DataIndex.Vitalshard,
        VitalStone        = DataIndex.Vitalstone,
        VitalCrystal      = DataIndex.Vitalcrystal,
        VerdictiveShard   = DataIndex.Verdictiveshard,
        VerdictiveStone   = DataIndex.Verdictivestone,
        VerdictiveCrystal = DataIndex.Verdictivecrystal,
        VotiveShard       = DataIndex.Votiveshard,
        VotiveStone       = DataIndex.Votivestone,
        VotiveCrystal     = DataIndex.Votivecrystal,
    }

    public static class MaterialTypeExtension
    {
        /// <summary>
        /// MaterialType enum은 DataIndex에서 발췌한 열거형.
        /// 따라서 강제 형변환도 가능하다.
        /// </summary>
        public static DataIndex ConvertToDataIndex(this MaterialType materialType)
        {
            return (DataIndex)materialType;
        }

        public static MaterialType ConvertToMaterialType(this DataIndex dataIndex)
        {
            if (dataIndex.GetCategory() != DataIndex.Material)
            {
                Debug.LogError($"Can't Convert {dataIndex} to MaterialType. return None");
                return MaterialType.None;
            }

            return (MaterialType)dataIndex;
        }
    }
}
