namespace Common
{
    public interface ICombatTaker : ICombatStatContainer
    {
        bool IsAlive { get; set; }
        float Hp { get; set; }
        // float Shield { get; set; }

        void TakeDamage(ICombatProvider provider);
        void TakeSpell(ICombatProvider provider);
        void TakeHeal(ICombatProvider provider);
        void TakeStatusEffect(ICombatProvider provider);
    }
}
