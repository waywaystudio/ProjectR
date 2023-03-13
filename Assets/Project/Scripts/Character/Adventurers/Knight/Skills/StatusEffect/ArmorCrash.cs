using System.Collections;
using Common;
using Common.StatusEffect;
using UnityEngine;

namespace Character.Adventurers.Knight.Skills.StatusEffect
{
    public class ArmorCrash : StatusEffectComponent
    {
        [SerializeField] private ArmorValue armorValue;
        

        public override void OnOverride()
        {
            ProgressTime.Value += duration;
        }
        
        protected override void Complete()
        {
            Taker.StatTable.Unregister(ActionCode, armorValue);
            
            base.Complete();
        }

        
        protected override IEnumerator Effectuating()
        {
            ProgressTime.Value = duration;

            Taker.StatTable.Register(ActionCode, armorValue);

            while (ProgressTime.Value > 0f)
            {
                ProgressTime.Value -= Time.deltaTime;
                yield return null;
            }

            Complete();
        }
    }
}
