namespace Core
{
    public interface ICombatTaker
    {
        bool IsAlive { get; set; }
        UnityEngine.GameObject Taker { get; }
        
        void TakeDamage(ICombatProvider combatInfo);
        void TakeHeal(ICombatProvider healInfo);
        void TakeBuff(string key, ICombatProvider statusEffect);
        void TakeDeBuff(string key, ICombatProvider statusEffect);
    }
}
