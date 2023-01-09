using System.Collections;
using Core;
using UnityEngine;

namespace Character.Combat.StatusEffects.Buff
{
    public class BloodDrainBuff : StatusEffectBehaviour
    {
        private WaitForSeconds waitForSeconds;
        
        protected override IEnumerator Initiate()
        {
            waitForSeconds = new WaitForSeconds(Duration);

            Provider.OnCombatActive.Register(InstanceID, BloodDrain);

            yield return waitForSeconds;
            
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
