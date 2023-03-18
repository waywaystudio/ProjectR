using Common;
using Common.StatusEffect;
using UnityEngine;

namespace Adventurers.Knight.StatusEffect
{
    public class ArmorCrash : StatusEffectComponent
    {
        [SerializeField] private ArmorValue armorValue;

        public override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);
            
            OnActivated.Register("StatTableRegister", () => Taker.StatTable.Register(ActionCode, armorValue));
            OnCompleted.Register("Return", () => Taker.StatTable.Unregister(ActionCode, armorValue));
        }


        private void Update()
        {
            if (ProgressTime.Value > 0)
            {
                ProgressTime.Value -= Time.deltaTime;
            }
            else
            {
                Complete();
            }
        }
    }
}
