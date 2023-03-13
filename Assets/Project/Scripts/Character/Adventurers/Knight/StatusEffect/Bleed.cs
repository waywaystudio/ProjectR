using System.Collections;
using Character;
using Common;
using Common.Completion;
using Common.StatusEffect;
using UnityEngine;

namespace Adventurers.Knight.StatusEffect
{
    public class Bleed : StatusEffectComponent
    {
        [SerializeField] protected PowerCompletion tickPower;
        [SerializeField] protected float interval;
        
        public override void Active(ICombatProvider provider, ICombatTaker taker)
        {
            base.Active(provider, taker);
            
            tickPower.Initialize(provider, ActionCode);
        }
        
        public override void OnOverride()
        {
            ProgressTime.Value += duration;
        }

        protected override IEnumerator Effectuating()
        {
            var hastedTick = interval * CharacterUtility.GetHasteValue(Provider.StatTable.Haste);

            ProgressTime.Value = duration;

            while (ProgressTime.Value > 0)
            {
                var tickBuffer = hastedTick;

                while (tickBuffer > 0f)
                {
                    ProgressTime.Value -= Time.deltaTime;
                    tickBuffer         -= Time.deltaTime;
                    
                    yield return null;
                }
                
                tickPower.Damage(Taker);
            }

            Complete();
        }
    }
}
