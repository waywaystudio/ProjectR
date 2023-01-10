using System.Collections;
using Core;
using UnityEngine;

namespace Character.Combat.StatusEffects.DeBuff
{
    public class FireballDeBuff : StatusEffectObject
    {
        [SerializeField] private ArmorValue armorValue;
        
        private WaitForSeconds waitForSeconds;
        
        protected override IEnumerator Initiate()
        {
            waitForSeconds = new WaitForSeconds(Duration);

            Taker.StatTable.Register(ActionCode, armorValue);

            yield return waitForSeconds;

            Taker.StatTable.Unregister(ActionCode, StatCode.Armor);
            Callback?.Invoke();
        }
        
        
#if UNITY_EDITOR
        public override void SetUp()
        {
            base.SetUp();

            armorValue.Value = CombatValue;
        }
#endif
    }
}
