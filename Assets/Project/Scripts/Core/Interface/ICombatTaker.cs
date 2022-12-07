namespace Core
{
    public interface ICombatTaker
    {
        UnityEngine.GameObject Taker { get; }
        
        void TakeDamage(IDamageProvider damageInfo);
        void TakeHeal(IHealProvider healInfo);
        void TakeExtra(IExtraProvider extra);
    }
}
