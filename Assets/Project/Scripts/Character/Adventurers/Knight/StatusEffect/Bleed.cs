using Common;
using Common.Execution;
using Common.StatusEffect;
using UnityEngine;

namespace Character.Adventurers.Knight.StatusEffect
{
    public class Bleed : StatusEffectComponent
    {
        [SerializeField] protected StatusEffectDamageExecution tickDamage;
        [SerializeField] protected float interval;
        
        private float hasteWeight;
        private float tickBuffer;

        
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
                    tickDamage.Execution(Taker);
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
