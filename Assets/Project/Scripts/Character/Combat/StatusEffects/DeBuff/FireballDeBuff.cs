using System.Collections;
using Core;
using UnityEngine;

namespace Character.Combat.StatusEffects.DeBuff
{
    public class FireballDeBuff : StatusEffectObject
    {
        [SerializeField] private ArmorValue armorValue;

        protected override IEnumerator Effectuate()
        {
            WaitBuffer = new WaitForSeconds(Duration);

            Taker.StatTable.Register(ActionCode, armorValue);

            yield return WaitBuffer;

            Taker.StatTable.Unregister(ActionCode, StatCode.Armor);
            Complete();
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
