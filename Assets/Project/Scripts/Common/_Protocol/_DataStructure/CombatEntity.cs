namespace Common
{
    public class CombatEntity
    {
        public bool IsCritical;
        public bool IsFinishedAttack;
        public float Value;
        public readonly ICombatTaker Taker;

        public CombatEntity() : this(null) { }
        public CombatEntity(ICombatTaker taker)
        {
            Taker       = taker;
        }
    }
}