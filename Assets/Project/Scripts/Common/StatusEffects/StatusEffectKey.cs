namespace Common.StatusEffects
{
    public class StatusEffectKey
    {
        public ICombatProvider Provider { get; }
        public DataIndex ActionCode { get; }

        public StatusEffectKey(StatusEffect statusEffect)
        {
            Provider   = statusEffect.Provider;
            ActionCode = statusEffect.DataIndex;
        }
        public StatusEffectKey(ICombatProvider provider, DataIndex actionCode)
        {
            Provider   = provider;
            ActionCode = actionCode;
        }

        // Override GetHashCode and Equals for proper dictionary key handling
        public override int GetHashCode()
        {
            return (Provider, ActionCode).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is StatusEffectKey other)
            {
                return other.Provider == Provider && other.ActionCode.Equals(ActionCode);
            }
            return false;
        }
    }
}
