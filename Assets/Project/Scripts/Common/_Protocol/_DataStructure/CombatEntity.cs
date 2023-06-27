namespace Common
{
    public class CombatEntity
    {
        public bool IsCritical;
        public bool IsFinishedAttack;
        public float Value;
        public readonly DataIndex CombatIndex;
        public readonly ICombatTaker Taker;

        public CombatEntity() : this(null) { }
        public CombatEntity(ICombatTaker taker) : this(DataIndex.None, taker) { }
        public CombatEntity(DataIndex combatIndex, ICombatTaker taker)
        {
            CombatIndex = combatIndex;
            Taker       = taker;
        }
    }
}