using System.Collections;
using Core;
using UnityEngine;

namespace Character.Combat.StatusEffects.Buff
{
    public class BloodDrainBuff : StatusEffectObject
    {
        private WaitForSeconds waitCache;
        
        protected override IEnumerator Initiate()
        {
            waitCache = new WaitForSeconds(Duration);

            Provider.OnCombatActive.Register(InstanceID, BloodDrain);

            yield return waitCache;
            
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
