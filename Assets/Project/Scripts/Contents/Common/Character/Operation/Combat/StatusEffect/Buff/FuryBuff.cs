using System.Collections;
using UnityEngine;

namespace Common.Character.Operation.Combat.StatusEffect.Buff
{
    public class FuryBuff : BaseStatusEffect
    {
        private WaitForSeconds waitForSeconds;
        
        public override IEnumerator MainAction()
        {
            waitForSeconds = new WaitForSeconds(Duration);

            Sender.StatTable.Register(StatCode.AddHaste, ID, CombatValue);

            yield return waitForSeconds;
            
            Sender.StatTable.Unregister(StatCode.AddHaste, ID);
            Callback?.Invoke();
        }
    }
}
