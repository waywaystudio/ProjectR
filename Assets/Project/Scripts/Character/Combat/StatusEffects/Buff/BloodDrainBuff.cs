using System.Collections;
using UnityEngine;

namespace Character.Combat.StatusEffects.Buff
{
    public class BloodDrainBuff : BaseStatusEffect
    {
        private WaitForSeconds waitForSeconds;
        
        public override IEnumerator MainAction()
        {
            waitForSeconds = new WaitForSeconds(Duration);

            Provider.OnCombatActive.Register((int)ActionCode, BloodDrain);

            yield return waitForSeconds;
            
            Provider.OnCombatActive.Unregister((int)ActionCode);
            Callback?.Invoke();
        }
        
        public void BloodDrain(CombatLog log)
        {
            var drainValue = log.Value * CombatValue;
            
            Provider.Status.Hp += drainValue;
        }
    }
}
