namespace Common
{
    public enum EthosType
    {
        None            = DataIndex.None           ,
        Vice            = DataIndex.Vice           ,
        Virtue          = DataIndex.Virtue         ,
        Deficiency      = DataIndex.Deficiency     ,
        Mean            = DataIndex.Mean           ,
        Excess          = DataIndex.Excess         ,
        Vacillation     = DataIndex.Vacillation    ,
        Fortitude       = DataIndex.Fortitude      ,
        Obstinacy       = DataIndex.Obstinacy      ,
        Cowardice       = DataIndex.Cowardice      ,
        Valor           = DataIndex.Valor          ,
        Recklessness    = DataIndex.Recklessness   ,
        Rash            = DataIndex.Rash           ,
        Cunning         = DataIndex.Cunning        ,
        Obsession       = DataIndex.Obsession      ,
        Conventionality = DataIndex.Conventionality,
        Creativity      = DataIndex.Creativity     ,
        Reverie         = DataIndex.Reverie        ,
        Ignorance       = DataIndex.Ignorance      ,
        Wisdom          = DataIndex.Wisdom         ,
        Arrogance       = DataIndex.Arrogance      ,
        Apathy          = DataIndex.Apathy         ,
        Benevolence     = DataIndex.Benevolence    ,
        Voracity        = DataIndex.Voracity       ,
    }

    public static class EthosExtension
    {
        public static bool IsVice(this       EthosType ethosType) => ((int)ethosType - (int)DataIndex.Vice) % ((int)DataIndex.Mean - (int)DataIndex.Vice) != 0;
        public static bool IsVirtue(this     EthosType ethosType) => ((int)ethosType - (int)DataIndex.Vice) % ((int)DataIndex.Mean - (int)DataIndex.Vice) == 0;
        public static bool IsDeficiency(this EthosType ethosType) => ((int)ethosType - (int)DataIndex.Vice) % ((int)DataIndex.Mean - (int)DataIndex.Vice) == 2;
        public static bool IsExcess(this     EthosType ethosType) => ((int)ethosType - (int)DataIndex.Vice) % ((int)DataIndex.Mean - (int)DataIndex.Vice) == 1;
    }
}
