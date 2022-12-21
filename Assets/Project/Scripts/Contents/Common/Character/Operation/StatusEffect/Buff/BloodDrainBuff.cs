using System.Collections;
using UnityEngine;

namespace Common.Character.Operation.StatusEffect.Buff
{
    public class BloodDrainBuff : BaseStatusEffect
    {
        private CharacterBehaviour Cb => ProviderInfo.Object.GetComponent<CharacterBehaviour>();
        private WaitForSeconds waitForSeconds;
        
        public override IEnumerator MainAction()
        {
            waitForSeconds = new WaitForSeconds(Duration);
            
            var cb = ProviderInfo.Object.GetComponent<CharacterBehaviour>();
            cb.OnCombatReporting.Register(BaseData.ID, BloodDrain);

            yield return waitForSeconds;
            
            cb.OnCombatReporting.Unregister(BaseData.ID);
            Callback?.Invoke();
        }
        
        public void BloodDrain(CombatLog log)
        {
            var drainValue = log.Value * BaseData.CombatValue;
            
            Cb.Hp += drainValue;
        }
    }
}
