using Common;
using Common.Completion;
using Common.StatusEffect;
using UnityEngine;

namespace Adventurers.Knight.StatusEffect
{
    public class Bleed : StatusEffectComponent
    {
        [SerializeField] protected DamageCompletion tickDamage;
        [SerializeField] protected float interval;
        
        private float hasteWeight;
        private float tickBuffer;
        
        public override void Initialized(ICombatProvider provider)
        {
            base.Initialized(provider);
            
            tickDamage.Initialize(provider, ActionCode);
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
                    tickDamage.Damage(Taker);
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
