namespace Common
{
    public enum ViceMaterialType
    {
        None = 0,
        ShardOfVacillation = DataIndex.ShardOfVacillation, // 51010101
        ShardOfCowardice = DataIndex.ShardOfCowardice, // 51010104
        ShardOfRash = DataIndex.ShardOfRash, // 51020107
        ShardOfConventionality = DataIndex.ShardOfConventionality, // 51020110
        ShardOfIgnorance = DataIndex.ShardOfIgnorance, // 51030113
        ShardOfApathy = DataIndex.ShardOfApathy, // 51030116
        StoneOfObstinacy = DataIndex.StoneOfObstinacy, // 51040303
        StoneOfRecklessness = DataIndex.StoneOfRecklessness, // 51040306
        StoneOfObsession = DataIndex.StoneOfObsession, // 51050309
        StoneOfReverie = DataIndex.StoneOfReverie, // 51050312
        StoneOfArrogance = DataIndex.StoneOfArrogance, // 51060315
        StoneOfVoracity = DataIndex.StoneOfVoracity, // 51060318
    }

    public enum GrowMaterial
    {
        None = 0,
        CrystalOfPathfinder = DataIndex.CrystalOfPathfinder, // 52000001
        ShardOfVicious = DataIndex.ShardOfVicious, // 52000102
        StoneOfVicious = DataIndex.StoneOfVicious, // 52000303
    }

    public static class ViceMaterialTypeExtension
    {
        public static DataIndex ToDataIndex(this ViceMaterialType type) => (DataIndex)type;
        public static ThemeType GetTheme(this ViceMaterialType type) => (ThemeType)type.ToDataIndex().GetSubCategoryBar();
        public static bool IsDeficiency(this ViceMaterialType type) => type.ToDataIndex().GetGroupBar() == 1;
        public static bool IsExcess(this ViceMaterialType type) => type.ToDataIndex().GetGroupBar()     == 3;
        public static int GetTier(this ViceMaterialType type) => type.ToDataIndex().GetIndexBar();
    }
}
