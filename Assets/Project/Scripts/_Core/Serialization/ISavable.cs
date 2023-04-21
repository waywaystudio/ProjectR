namespace Serialization
{
    public interface ISavable
    {        
        // ReSharper disable Unity.PerformanceAnalysis
        void Save();
        
        // ReSharper disable Unity.PerformanceAnalysis
        void Load();
    }
}