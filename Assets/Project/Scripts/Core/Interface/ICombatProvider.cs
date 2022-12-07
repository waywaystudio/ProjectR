namespace Core
{
    public interface IDamageProvider
    {
        UnityEngine.GameObject Provider { get; }
        
        double Value { get; }
        float Critical { get; }
        float Hit { get; }
    }

    public interface IHealProvider
    {
        double Value { get; }
        float Critical { get; }
    }

    public interface IExtraProvider
    {
        string Extra { get; }
    }
}
