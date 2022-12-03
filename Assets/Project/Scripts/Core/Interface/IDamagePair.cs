namespace Core
{
    public interface IDamageProvider
    {
        double Value { get; }
        float Critical { get; }
        float Hit { get; }
    }
    
    public interface ICombatTaker
    {
        UnityEngine.GameObject TargetObject { get; }
        void TakeDamage(IDamageProvider damageInfo);
        void TakeHeal(IHealProvider healInfo);
        void TakeExtra();
    }
}
