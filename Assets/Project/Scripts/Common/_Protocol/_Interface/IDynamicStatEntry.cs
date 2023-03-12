namespace Common
{
    public interface IDynamicStatEntry
    {
        AliveValue Alive { get; }
        HpValue Hp { get; }
        ResourceValue Resource { get; }
        ShieldValue Shield { get; }
        StatTable StatTable { get; }
        StatusEffectTable BuffTable { get; }
        StatusEffectTable DeBuffTable { get; }
    }
}