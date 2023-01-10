using System.Collections;
using Core;
using UnityEngine;

namespace Character.Combat.StatusEffects.Buff
{
    public class FuryBuff : StatusEffectObject
    {
        [SerializeField] private HasteValue hasteValue;
        
        private WaitForSeconds waitForSeconds;
        
        protected override IEnumerator Initiate()
        {
            waitForSeconds = new WaitForSeconds(Duration);

            Provider.StatTable.Register(ActionCode, hasteValue);

            yield return waitForSeconds;

            Provider.StatTable.Unregister(ActionCode, StatCode.Haste);
            Callback?.Invoke();
        }
        
        
#if UNITY_EDITOR
        public override void SetUp()
        {
            base.SetUp();

            hasteValue.Value = CombatValue;
        }
#endif
    }
}
