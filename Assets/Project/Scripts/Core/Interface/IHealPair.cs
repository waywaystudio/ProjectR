namespace Core
{
    public interface IHealProvider
    {
        double Value { get; }
        float Critical { get; }
    }

    public interface IHealTaker
    {
        double Hp { get; set; }
        float AdditionalValue { get; }
    }
}
