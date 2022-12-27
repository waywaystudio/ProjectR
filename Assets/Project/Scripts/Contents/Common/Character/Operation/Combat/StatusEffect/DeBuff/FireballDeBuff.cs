using System.Collections;
using UnityEngine;

namespace Common.Character.Operation.Combat.StatusEffect.DeBuff
{
    public class FireballDeBuff : BaseStatusEffect
    {
        private WaitForSeconds waitForSeconds;
        private CharacterBehaviour cb;
        
        public override IEnumerator MainAction()
        {
            waitForSeconds = new WaitForSeconds(Duration);

            cb ??= TakerInfo.Object.GetComponent<CharacterBehaviour>();
            cb.StatTable.Register(StatCode.MultiArmor, ID, () => 0.95f);

            yield return waitForSeconds;
            
            cb.StatTable.Unregister(StatCode.MultiArmor, ID);
            Callback?.Invoke();
        }
    }
}
