using System.Collections;
using UnityEngine;

namespace Character.Combat.StatusEffects.Buff
{
    public class FuryBuff : BaseStatusEffect
    {
        private WaitForSeconds waitForSeconds;
        
        public override IEnumerator MainAction()
        {
            waitForSeconds = new WaitForSeconds(Duration);

            Provider.StatTable.Register(StatCode.AddHaste, (int)ActionCode, CombatValue);

            yield return waitForSeconds;
            
            Provider.StatTable.Unregister(StatCode.AddHaste, (int)ActionCode);
            Callback?.Invoke();
        }
    }
}
