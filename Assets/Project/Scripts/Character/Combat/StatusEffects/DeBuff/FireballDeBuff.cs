using System.Collections;
using Core;
using UnityEngine;

namespace Character.Combat.StatusEffects.DeBuff
{
    public class FireballDeBuff : BaseStatusEffect
    {
        private WaitForSeconds waitForSeconds;
        
        public override IEnumerator MainAction()
        {
            waitForSeconds = new WaitForSeconds(Duration);

            TakerInfo.StatTable.Register(ActionCode, new ArmorValue(-100));

            yield return waitForSeconds;

            TakerInfo.StatTable.Unregister(ActionCode, StatCode.Armor);
            Callback?.Invoke();
        }
    }
}
