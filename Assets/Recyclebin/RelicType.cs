// namespace Common
// {
//     public enum RelicType
//     {
//         None     = 0,
//         // VowedRelic    = DataIndex.VowedRelic, // 14010001,
//         // ValiantRelic = DataIndex.ValiantRelic, // 14020002,
//         // VeteransRelic = DataIndex.VeteransRelic, // 14030003,
//         // VisionedRelic    = DataIndex.VisionedRelic, // 14040004,
//         // VeritableRelic = DataIndex.VeritableRelic, // 14050005,
//         // VenerableRelic   = DataIndex.VenerableRelic, // 14060006,
//     }
//
//     public static class RelicExtension
//     {
//         public static DataIndex ConvertToDataIndex(this RelicType relicType) => (DataIndex)relicType;
//         public static int GetSubCategoryIndex(this RelicType type) => type.ConvertToDataIndex().GetSubCategoryBar();
//         public static ThemeType GetTheme(this RelicType type)
//         {
//             var ethosGroupIndex = type.GetSubCategoryIndex();
//             var themeType = ethosGroupIndex.FindIndex<ThemeType>();
//
//             return themeType;
//         }
//     }
// }
