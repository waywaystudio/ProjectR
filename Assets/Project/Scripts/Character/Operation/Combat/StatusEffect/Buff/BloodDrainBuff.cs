using System.Collections;
using UnityEngine;

namespace Common.Character.Operation.Combat.StatusEffect.Buff
{
    public class BloodDrainBuff : BaseStatusEffect
    {
        private CharacterBehaviour cb;
        
        private CharacterBehaviour Cb => cb ??= Sender.Object.GetComponent<CharacterBehaviour>();
        private WaitForSeconds waitForSeconds;
        
        public override IEnumerator MainAction()
        {
            waitForSeconds = new WaitForSeconds(Duration);

            Cb.OnCombatActive.Register(BaseData.ID, BloodDrain);

            yield return waitForSeconds;
            
            Cb.OnCombatActive.Unregister(BaseData.ID);
            Callback?.Invoke();
        }
        
        public void BloodDrain(CombatLog log)
        {
            var drainValue = log.Value * BaseData.CombatValue;
            
            Cb.Status.Hp += drainValue;
        }
    }
}
