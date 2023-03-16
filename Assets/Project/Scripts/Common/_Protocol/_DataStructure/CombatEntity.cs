namespace Common
{
    public class CombatEntity
    {
        public bool IsCritical;
        public bool IsFinishedAttack;
        public float Value;
        public readonly ICombatTable CombatTable;
        public readonly ICombatTaker Taker;

        public CombatEntity() : this(null, null) { }
        public CombatEntity(ICombatTable combatTable, ICombatTaker taker)
        {
            CombatTable = combatTable;
            Taker       = taker;
        }
    }
}