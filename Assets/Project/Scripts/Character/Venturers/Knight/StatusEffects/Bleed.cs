using Common;
using Common.StatusEffects;
using UnityEngine;

namespace Character.Venturers.Knight.StatusEffects
{
    public class Bleed : StatusEffect
    {
        [SerializeField] protected float interval;
        
        private float hasteWeight;
        private float tickBuffer;

        public override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);

            SequenceBuilder.Add(SectionType.Active, "SetHasteWeightAndTickBuffer", 
                                () => hasteWeight = tickBuffer =
                                    interval * CombatFormula.GetHasteValue(Provider.StatTable.Haste));
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
                    executor.Execute(Taker);
                    tickBuffer = hasteWeight;
                }
            }
            else
            {
                SequenceInvoker.Complete();
            }
        }
    }
}
