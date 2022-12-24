using System.Collections;
using UnityEngine;

namespace Common.Character.Operation.Combat.StatusEffect.Buff
{
    public class BloodDrainBuff : BaseStatusEffect
    {
        private CharacterBehaviour cb;
        
        private CharacterBehaviour Cb => cb ??= ProviderInfo.Object.GetComponent<CharacterBehaviour>();
        private WaitForSeconds waitForSeconds;
        
        public override IEnumerator MainAction()
        {
            waitForSeconds = new WaitForSeconds(Duration);

            Cb.OnCombatReporting.Register(BaseData.ID, BloodDrain);

            yield return waitForSeconds;
            
            Cb.OnCombatReporting.Unregister(BaseData.ID);
            Callback?.Invoke();
        }
        
        public void BloodDrain(CombatLog log)
        {
            var drainValue = log.Value * BaseData.CombatValue;
            
            Cb.Hp += drainValue;
        }
    }
}
