using System;
using System.Collections;
using Common;
using Common.StatusEffect;
using UnityEngine;

namespace Adventurers.Knight.StatusEffect
{
    public class ArmorCrash : StatusEffectComponent
    {
        [SerializeField] private ArmorValue armorValue;

        public override void OnOverride()
        {
            ProgressTime.Value += duration;
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

        private void OnEnable()
        {
            OnCompleted.Register("Return", () => Taker.StatTable.Unregister(ActionCode, armorValue));
        }

        private void OnDisable()
        {
            OnCompleted.Unregister("Return");
        }
    }
}
