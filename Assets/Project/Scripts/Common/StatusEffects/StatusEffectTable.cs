namespace Common.StatusEffects
{
    public class StatusEffectTable : Table<DataIndex, StatusEffect>
    {
        public ActionTable<StatusEffect> OnEffectAdded { get; } = new();
        public ActionTable<StatusEffect> OnEffectRemoved { get; } = new();
        

        public override void Add(DataIndex key, StatusEffect statusEffect)
        {
            base.Add(key, statusEffect);
            
            OnEffectAdded?.Invoke(statusEffect);
        }
        
        public override bool Remove(DataIndex effectIndex)
        {
            if (!TryGetValue(effectIndex, out var effect)) return false;

            OnEffectRemoved?.Invoke(effect);
            base.Remove(effectIndex);

            return true;
        }

        public bool TryGetEffect<T>(DataIndex key, out T effect) where T : StatusEffect
        {
            if (Map.TryGetValue(key, out var value))
            {
                var result = value as T;

                effect = result;
                return true;
            }

            effect = null;
            return false;
        }
    }
}