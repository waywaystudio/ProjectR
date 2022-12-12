namespace Core
{
    public interface ICombatTaker
    {
        bool IsAlive { get; set; }
        UnityEngine.GameObject Taker { get; }
        
        void TakeDamage(IDamageProvider damageInfo);
        void TakeHeal(IHealProvider healInfo);
        void TakeExtra(IExtraProvider extra);
    }
}
