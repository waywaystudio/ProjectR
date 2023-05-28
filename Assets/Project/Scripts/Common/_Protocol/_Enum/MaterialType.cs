namespace Common
{
    public enum MaterialType
    {
        // Theme
        None              = 0,
        VowedShard        = DataIndex.VowShard,              // 51010001
        VowedStone        = DataIndex.VowStone,              // 51010002
        VowedCrystal      = DataIndex.VowCrystal,            // 51010003
        ValianceShard     = DataIndex.ValianceShard,         // 51020001
        ValianceStone     = DataIndex.ValianceStone,         // 51020002
        ValianceCrystal   = DataIndex.ValianceCrystal,       // 51020003
        VeteranShard     = DataIndex.VeteranShard,           // 51030001
        VeteranStone     = DataIndex.VeteranStone,           // 51030002
        VeteranCrystal   = DataIndex.VeteranCrystal,         // 51030003
        VisionShard        = DataIndex.VisionShard,          // 51040001
        VisionStone        = DataIndex.VisionStone,          // 51040002
        VisionCrystal      = DataIndex.VisionCrystal,        // 51040003
        VerityShard   = DataIndex.VerityShard,               // 51050001
        VerityStone   = DataIndex.VerityStone,               // 51050002
        VerityCrystal = DataIndex.VerityCrystal,             // 51050003
        VenerationShard       = DataIndex.VenerationShard,   // 51060001
        VenerationStone       = DataIndex.VenerationStone,   // 51060002
        VenerationCrystal     = DataIndex.VenerationCrystal, // 51060003
        
        // Conversion & Enchant
        VirtuousShard     = DataIndex.VirtuousShard,         // 51070001
        VirtuousStone     = DataIndex.VirtuousStone,         // 51070002
        VirtuousCrystal   = DataIndex.VirtuousCrystal,       // 51070003
        ViciousShard      = DataIndex.ViciousShard,          // 51080001
        ViciousStone      = DataIndex.ViciousStone,          // 51080002
        ViciousCrystal    = DataIndex.ViciousCrystal,        // 51080003
    }

    public static class MaterialTypeExtension
    {
        public static DataIndex ToDataIndex(this MaterialType type) => (DataIndex)type;
        
        public static int GetTier(this MaterialType type) => type.ToDataIndex().GetIndexBar();
        public static MaterialType GetUpTier(this MaterialType type) => (MaterialType)UnityEngine.Mathf.Min((int)type, (int)type  + 1);
        public static MaterialType GetDownTier(this MaterialType type) => (MaterialType)UnityEngine.Mathf.Max((int)type, (int)type - 1);
        
        public static ThemeType GetTheme(this MaterialType type)
        {
            var ethosGroupIndex = type.ToDataIndex().GetSubCategoryBar();
            var themeType = ethosGroupIndex.FindIndex<ThemeType>();

            return themeType;
        }
        public static MaterialType GetNextTheme(this MaterialType type)
        {
            if (type is MaterialType.None or > MaterialType.VenerationCrystal)
            {
                UnityEngine.Debug.LogError($"Not None and Must be Theme Material. Input:{type.ToString()}");
                return MaterialType.None;
            }
            
            var detachedTheme = (int)type - (int)type.GetTheme() * 10000;
            var nextTheme = type.GetTheme().GetNextTheme();

            return (MaterialType)(detachedTheme + (int)nextTheme * 10000);
        }
    }
}
