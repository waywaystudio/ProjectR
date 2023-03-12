namespace Common
{
    public class CombatEntity
    {
        public bool IsCritical;
        public bool IsFinishedAttack;
        public float Value;
        public ICombatTable CombatTable;
        public ICombatTaker Taker;

        public CombatEntity() : this(null, null) { }
        public CombatEntity(ICombatTable combatTable, ICombatTaker taker)
        {
            CombatTable = combatTable;
            Taker       = taker;
        }
    }

    public class StatusEffectEntity
    {
        public bool IsOverride;
        public readonly IStatusEffect Effect;
        public ICombatTaker Taker;

        public StatusEffectEntity(IStatusEffect effect, ICombatTaker taker)
        {
            Effect = effect;
            Taker  = taker;
        }
    }
}