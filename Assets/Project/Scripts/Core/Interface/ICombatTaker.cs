namespace Core
{
    public interface ICombatTaker
    {
        bool IsAlive { get; set; }
        UnityEngine.GameObject Taker { get; }
        
        void TakeDamage(ICombatProvider combatInfo);
        void TakeHeal(ICombatProvider healInfo);
        void TakeStatusEffect(ICombatProvider statusEffect);
    }
}
