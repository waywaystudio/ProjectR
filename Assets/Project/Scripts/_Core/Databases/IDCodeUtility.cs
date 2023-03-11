namespace Databases
{
    public static class IDCodeUtility
    {
        public static int GetCategoryIndexByID(DataIndex dataIndex) => (int)((int)dataIndex * 0.000001f);
        public static int GetCategoryIndexByID(int idCode) => (int)(idCode * 0.000001f);
        public static DataIndex ConvertByName(string name) => name.ToEnum<DataIndex>();
    }
}
