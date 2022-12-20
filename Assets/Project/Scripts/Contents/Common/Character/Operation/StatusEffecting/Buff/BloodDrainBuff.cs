using System.Collections;
using UnityEngine;

namespace Common.Character.Operation.StatusEffecting.Buff
{
    public class BloodDrainBuff : BaseStatusEffect
    {
        private CharacterBehaviour Cb => ProviderInfo.Provider.GetComponent<CharacterBehaviour>();
        private WaitForSeconds waitForSeconds;
        
        public override IEnumerator MainAction()
        {
            waitForSeconds = new WaitForSeconds(Duration);
            
            var cb = ProviderInfo.Provider.GetComponent<CharacterBehaviour>();
            cb.OnReportDamage.Register(BaseData.ID, BloodDrain);

            yield return waitForSeconds;
            
            cb.OnReportDamage.UnRegister(BaseData.ID);
            Callback?.Invoke();
        }
        
        public void BloodDrain(CombatLog log)
        {
            var drainValue = log.Value * BaseData.CombatValue;
            
            Cb.Hp += drainValue;
        }
    }
}
