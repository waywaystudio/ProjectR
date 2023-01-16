using System.Collections;
using Core;
using UnityEngine;

namespace Character.Combat.StatusEffects.DeBuff
{
    public class CorruptionDeBuff : StatusEffectObject, ICombatTable
    {
        [SerializeField] private PowerValue tickValue;
        [SerializeField] private int tickCount = 5;

        private float tick;

        public StatTable StatTable { get; } = new();
        public override float Duration => duration * CharacterUtility.GetHasteValue(StatTable.Haste);
        

        public override void Effectuate(ICombatProvider provider, ICombatTaker taker)
        {
            base.Effectuate(provider, taker);
            
            StatTable.Register(ActionCode, tickValue);
            StatTable.UnionWith(Provider.StatTable);

            tick = Time.deltaTime;
        }

        protected override IEnumerator Initiate()
        {
            var tickInterval = Duration / tickCount;
            var currentTick = tickInterval;

            for (var i = 0; i < tickCount; ++i)
            {
                while (currentTick > 0)
                {
                    currentTick -= tick;
                    yield return null;
                }

                if (DamageModule) Taker.TakeDamage(DamageModule);
                
                currentTick = tickInterval;
            }
            
            Callback?.Invoke();
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
