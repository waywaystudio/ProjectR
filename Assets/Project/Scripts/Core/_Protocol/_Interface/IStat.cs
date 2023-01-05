namespace Core
{
    public interface IDynamicStatEntry
    {
        AliveValue IsAlive { get; }
        HpValue Hp { get; }
        ResourceValue Resource { get; }
        ShieldValue Shield { get; }
    }
}
