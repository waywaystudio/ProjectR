namespace Core
{
    public enum ModuleType
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
        
        /* StatusEffect */
        Dot,
        Feedback,

        /* Projector */
        Shape,
        
        All = int.MaxValue
    }
}
