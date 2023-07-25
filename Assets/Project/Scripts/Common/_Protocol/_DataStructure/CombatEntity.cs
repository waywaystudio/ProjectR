namespace Common
{
    public class CombatEntity
    {
        public CombatEntityType Type;
        public bool IsCritical;
        public float Value;
        public readonly ICombatProvider Provider;
        public readonly ICombatTaker Taker;
        public readonly DataIndex CombatIndex;

        public CombatEntity(ICombatProvider provider, DataIndex combatIndex, ICombatTaker taker)
        {
            Provider    = provider;
            CombatIndex = combatIndex;
            Taker       = taker;
            Type        = CombatEntityType.None;
        }
    }
}