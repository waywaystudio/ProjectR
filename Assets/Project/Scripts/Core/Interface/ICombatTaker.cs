namespace Core
{
    public interface ICombatTaker
    {
        bool IsAlive { get; set; }
        UnityEngine.GameObject Object { get; }
        
        void TakeDamage(ICombatProvider provider);
        void TakeHeal(ICombatProvider provider);
        void TakeBuff(ICombatProvider provider);
        void TakeDeBuff(ICombatProvider provider);
    }
}
