namespace Common.Character.Skills.Entity
{
    [System.Flags]
    public enum EntityType
    {
        None = 0,
        Damage = 1 << 0,
        CoolTime = 1 << 1,
        Range = 1 << 2,
        Casting = 1 << 3,
        Target = 1  << 4,
    
        All = int.MaxValue
    }
}
