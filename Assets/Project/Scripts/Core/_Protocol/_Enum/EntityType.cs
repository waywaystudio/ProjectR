namespace Core
{
    [System.Flags]
    public enum EntityType
    {
        None = 0,
        Damage = 1 << 0,
        CoolTime = 1 << 1,
        Resource = 1 << 2,
        Casting = 1 << 3,
        Target = 1  << 4,
        Projectile = 1 << 5,
        Heal = 1 << 6,
        Area = 1 << 7,
        // DeBuff = 1 << 8,
        StatusEffect = 1 << 9,
        Dispel = 1 << 10,
    
        All = int.MaxValue
    }
}
