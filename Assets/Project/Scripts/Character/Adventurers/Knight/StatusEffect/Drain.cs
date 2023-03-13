using System.Collections;
using Common;
using Common.StatusEffect;
using UnityEngine;

namespace Adventurers.Knight.StatusEffect
{
    public class Drain : StatusEffectComponent
    {
        [SerializeField] private float drainPercentage;

        public override void OnOverride()
        {
            ProgressTime.Value += duration;
        }

        protected override void Complete()
        {
            Provider.OnProvideDamage.Unregister("DrainBuff");
            
            base.Complete();
        }

        protected override IEnumerator Effectuating()
        {
            ProgressTime.Value = duration;
            Provider.OnProvideDamage.Register("DrainBuff", DrainHp);
            
            while (ProgressTime.Value > 0f)
            {
                ProgressTime.Value -= Time.deltaTime;
                yield return null;
            }

            Complete();
        }

        private void DrainHp(CombatEntity entity)
        {
            Provider.DynamicStatEntry.Hp.Value += entity.Value * drainPercentage;
        }


#if UNITY_EDITOR
        public override void EditorSetUp()
        {
            base.EditorSetUp();
            
            var statusEffectData = Database.StatusEffectSheetData(ActionCode);

            drainPercentage = statusEffectData.ValueList[0];
        }
#endif
    }
}
