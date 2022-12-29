using Core;

namespace MainGame.Data
{
    public static class IDCodeUtility
    {
        public static int GetCategoryIndexByID(IDCode idCode) => (int)((int)idCode * 0.000001f);
        public static int GetCategoryIndexByID(int idCode) => (int)(idCode * 0.000001f);
        public static IDCode ConvertByName(string name) => name.ToEnum<IDCode>();
    }
}
