using System.Collections;
using Core;
using UnityEngine;

namespace Character.Combat.StatusEffects.DeBuff
{
    public class CorruptionDeBuff : StatusEffectBehaviour, ICombatEntity
    {
        [SerializeField] private PowerValue tickValue;
        [SerializeField] private int tickCount = 5;

        public IDynamicStatEntry DynamicStatEntry => Provider.DynamicStatEntry;
        public StatTable StatTable { get; } = new();
        public override float Duration => duration * CharacterUtility.GetHasteValue(StatTable.Haste);
        

        protected override IEnumerator Initiate()
        {
            var tickInterval = Duration / tickCount;
            var currentTick = tickInterval;

            for (var i = 0; i < tickCount; ++i)
            {
                while (currentTick > 0)
                {
                    currentTick -= Time.deltaTime;
                    yield return null;
                }

                Taker.TakeDamage(this);
                currentTick = tickInterval;
            }
            
            Callback?.Invoke();
        }


        private void Start()
        {
            StatTable.Register(ActionCode, tickValue);
            StatTable.UnionWith(Provider.StatTable);
        }


#if UNITY_EDITOR
        public override void SetUp()
        {
            base.SetUp();

            tickValue.Value = CombatValue;
        }
#endif
    }
}
