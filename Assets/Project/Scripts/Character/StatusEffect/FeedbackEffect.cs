using System.Collections;
using UnityEngine;

namespace Character.StatusEffect
{
    public class FeedbackEffect : StatusEffectComponent
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
            Provider.OnProvideDamage.Register("DrainBuff", Drain);
            
            while (ProgressTime.Value > 0f)
            {
                ProgressTime.Value -= Time.deltaTime;
                yield return null;
            }

            Complete();
        }

        private void Drain(CombatEntity entity)
        {
            Provider.DynamicStatEntry.Hp.Value += entity.Value * drainPercentage;
        }  
        

        public override void EditorSetUp()
        {
            base.EditorSetUp();
            
            var statusEffectData = Database.StatusEffectSheetData(ActionCode);

            drainPercentage = statusEffectData.ValueList[0];
        }
    }
}
