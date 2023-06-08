using Common;
using Common.StatusEffect;
using UnityEngine;

namespace Character.Venturers.Knight.StatusEffect
{
    public class Bleed : StatusEffectComponent
    {
        [SerializeField] protected float interval;
        
        private float hasteWeight;
        private float tickBuffer;

        
        public override void Activate(ICombatTaker taker)
        {
            base.Activate(taker);
            
            hasteWeight = tickBuffer = 
                interval * CombatFormula.GetHasteValue(Provider.StatTable.Haste);
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
                    ExecutionTable.Execute(Taker);
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
