using System.Collections;
using Core;
using UnityEngine;

namespace Character.Combat.StatusEffects.Buff
{
    public class FuryBuff : BaseStatusEffect
    {
        private WaitForSeconds waitForSeconds;
        
        public override IEnumerator MainAction()
        {
            waitForSeconds = new WaitForSeconds(Duration);

            Provider.StatTable.Register(ActionCode, new HasteValue(CombatValue));

            yield return waitForSeconds;

            Provider.StatTable.Unregister(ActionCode, StatCode.Haste);
            Callback?.Invoke();
        }
    }
}
