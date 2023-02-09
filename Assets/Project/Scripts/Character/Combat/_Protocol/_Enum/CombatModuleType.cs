namespace Character.Combat
{
    public enum CombatModuleType
    {
        None = 0,
        Damage,
        CoolTime,
        Resource,
        Casting,
        Target,
        Projectile,
        Heal,
        Projector,
        StatusEffect,
        Dispel,
        GlobalCoolTime,
        Colliding,
        Charging,
        Area,
        
        /* StatusEffect */
        Dot,
        Hot,
        StatModifier,
        Feedback,

        /* Projector */
        Shape,
        
        All = int.MaxValue
    }
}
