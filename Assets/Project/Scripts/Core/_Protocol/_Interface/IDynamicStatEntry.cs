namespace Core
{
    public interface IDynamicStatEntry
    {
        AliveValue IsAlive { get; }
        HpValue Hp { get; }
        ResourceValue Resource { get; }
        ShieldValue Shield { get; }
        StatTable StatTable { get; }
        StatusEffectTable BuffTable { get; }
        StatusEffectTable DeBuffTable { get; }
    }
}
