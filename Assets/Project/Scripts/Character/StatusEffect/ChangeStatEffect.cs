using System.Collections;
using Core;
using UnityEngine;

namespace Character.StatusEffect
{
    public class ChangeStatEffect : StatusEffectComponent
    {
        [SerializeField] private ArmorValue armorValue;
        

        public override void OnOverride()
        {
            ProcessTime.Value += duration;
        }

        protected override void Init() { }
        protected override IEnumerator Effectuating(ICombatTaker taker)
        {
            ProcessTime.Value = duration;
            
            taker.StatTable.Register(ActionCode, armorValue);

            while (ProcessTime.Value > 0f)
            {
                ProcessTime.Value -= Time.deltaTime;
                yield return null;
            }

            Complete();
        }
    }
}
