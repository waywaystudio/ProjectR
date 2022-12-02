namespace Core
{
    public interface IDamageProvider
    {
        double Value { get; }
        float Critical { get; }
        float Hit { get; }
    }
    
    public interface IDamageTaker
    {
        double Hp { get; set; }
        float Evade { get; set; }
        float AdditionalValue { get; set; }
    }
}
