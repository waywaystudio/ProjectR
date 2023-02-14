using System.Collections;
using Core;
using UnityEngine;

namespace Character.StatusEffect
{
    public class DamageOverTimeEffect : StatusEffectComponent, ICombatTable
    {
        [SerializeField] private float interval;
        [SerializeField] private PowerValue tickPowerValue;
        
        public StatTable StatTable { get; } = new();
        public FloatEvent ProcessTime { get; } = new(0f, float.MaxValue);
        

        protected override void Init()
        {
            OnEnded.Register("ResetProcess", () => ProcessTime.Value = 0f);
        }

        protected override IEnumerator Effectuating(ICombatTaker taker)
        {
            var hastedTick = interval * CharacterUtility.GetHasteValue(Provider.StatTable.Haste);
            var repeatCount = (int)(duration / hastedTick);

            for (var i = 0; i < repeatCount; ++i)
            {
                var tickBuffer = hastedTick;
                
                while (tickBuffer > 0)
                {
                    ProcessTime.Value += StatusEffectTick;
                    tickBuffer        -= StatusEffectTick;
                    
                    yield return null;
                }

                UpdateDoT();
                taker.TakeDamage(this);
            }
            
            Complete();
        }

        private void UpdateDoT()
        {
            StatTable.Clear();
            StatTable.Register(ActionCode, tickPowerValue);
            StatTable.UnionWith(Provider.StatTable);
        }
    }
}
