using System.Collections;
using UnityEngine;

namespace Character.Combat.StatusEffects.DeBuff
{
    public class FireballDeBuff : BaseStatusEffect
    {
        private WaitForSeconds waitForSeconds;
        
        public override IEnumerator MainAction()
        {
            waitForSeconds = new WaitForSeconds(Duration);

            TakerInfo.StatTable.Register(StatCode.MultiArmor, (int)ActionCode, () => 0.95f);

            yield return waitForSeconds;
            
            TakerInfo.StatTable.Unregister(StatCode.MultiArmor, (int)ActionCode);
            Callback?.Invoke();
        }
    }
}
