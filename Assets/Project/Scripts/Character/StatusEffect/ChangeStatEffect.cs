using System.Collections;
using Common;
using UnityEngine;

namespace Character.StatusEffect
{
    public class ChangeStatEffect : StatusEffectComponent
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
