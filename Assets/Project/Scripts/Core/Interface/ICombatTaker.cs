namespace Core
{
    public interface ICombatTaker
    {
        UnityEngine.GameObject TargetObject { get; }
        void TakeDamage(IDamageProvider damageInfo);
        void TakeHeal(IHealProvider healInfo);
        void TakeExtra(IExtraProvider extra);
    }
}
