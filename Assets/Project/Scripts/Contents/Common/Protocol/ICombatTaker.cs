using UnityEngine;

namespace Common
{
    public interface ICombatTaker
    {
        bool IsAlive { get; set; }
        string Name { get; }
        GameObject Object { get; }
        DefenseValueEntity DefenseValue { get; }

        void TakeDamage(ICombatProvider provider);
        void TakeSpell(ICombatProvider provider);
        void TakeHeal(ICombatProvider provider);
        void TakeStatusEffect(ICombatProvider provider);
    }
}
