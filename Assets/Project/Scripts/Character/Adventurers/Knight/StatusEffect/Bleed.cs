using Common;
using Common.Completion;
using Common.StatusEffect;
using UnityEngine;

namespace Adventurers.Knight.StatusEffect
{
    public class Bleed : StatusEffectSequence
    {
        [SerializeField] protected DamageCompletion tickDamage;
        [SerializeField] protected float interval;
        
        private float hasteWeight;
        private float tickBuffer;
        
        public override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);
            
            // tickDamage.Initialize(provider, ActionCode);
            tickDamage.Initialize(provider);
        }
        
        public override void Execution(ICombatTaker taker)
        {
            base.Execution(taker);
            
            hasteWeight = tickBuffer = 
                interval * CharacterUtility.GetHasteValue(Provider.StatTable.Haste);
        }
        

        private void Update()
        {
            if (ProgressTime.Value > 0)
            {
                if (tickBuffer > 0f)
                {
                    ProgressTime.Value -= Time.deltaTime;
                    tickBuffer         -= Time.deltaTime;
                }
                else
                {
                    tickDamage.Completion(Taker);
                    tickBuffer = hasteWeight;
                }
            }
            else
            {
                Complete();
            }
        }
    }
}
