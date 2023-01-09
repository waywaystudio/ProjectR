namespace Core
{
    public class StatusEffectTable : ComparableTable<(ICombatProvider, DataIndex), IStatusEffect>
    {
        public void Register(IStatusEffect statusEffect)
        {
            var key = (statusEffect.Provider, statusEffect.ActionCode);
            
            Register(key, statusEffect);
        }

        public void Unregister(IStatusEffect statusEffect) =>
            Unregister((statusEffect.Provider, statusEffect.ActionCode));
        
        public override int Compare(IStatusEffect original, IStatusEffect source)
        {
            /*
             * 같은 버프 or 디버프가 들어왔을 때 비교하는 기준 함수를 작성해야 한다.
             * 버프와 디버프 마다 다를려나...
             */
            return 1;
        }
    }
}
