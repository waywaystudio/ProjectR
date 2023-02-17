namespace Core
{
    public interface IDataSetUp
    {
#if UNITY_EDITOR
        void SetUp();
#endif
    }
}