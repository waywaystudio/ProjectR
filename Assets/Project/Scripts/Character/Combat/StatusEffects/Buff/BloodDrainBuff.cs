using System.Collections;
using Core;
using UnityEngine;

namespace Character.Combat.StatusEffects.Buff
{
    public class BloodDrainBuff : StatusEffectObject
    {
        protected override IEnumerator Initiate()
        {
            WaitBuffer = new WaitForSeconds(Duration);

            Provider.OnCombatActive.Register(InstanceID, BloodDrain);

            yield return WaitBuffer;
            
            Provider.OnCombatActive.Unregister(InstanceID);
            Callback?.Invoke();
        }
        
        public void BloodDrain(CombatLog log)
        {
            var drainValue = log.Value * CombatValue;
            
            Provider.DynamicStatEntry.Hp.Value += drainValue;
        }
    }
}
