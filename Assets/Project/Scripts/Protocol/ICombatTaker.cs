using Common.Character;

namespace Common
{
    public interface ICombatTaker : ICombatStatContainer
    {
        void TakeDamage(ICombatProvider provider);
        void TakeSpell(ICombatProvider provider);
        void TakeHeal(ICombatProvider provider);
        void TakeStatusEffect(ICombatProvider provider);
        void ReportPassive(CombatLog log);
    }
}
