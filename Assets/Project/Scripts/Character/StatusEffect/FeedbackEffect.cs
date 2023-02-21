using System.Collections;
using Core;
using UnityEngine;

namespace Character.StatusEffect
{
    public class FeedbackEffect : StatusEffectComponent
    {
        [SerializeField] private float drainPercentage;

        public override void OnOverride()
        {
            ProcessTime.Value += duration;
        }

        protected override void Init() { }

        protected override IEnumerator Effectuating(ICombatTaker taker)
        {
            ProcessTime.Value = duration;
            Provider.OnProvideCombat.Register("DrainBuff", Drain);
            
            while (ProcessTime.Value > 0f)
            {
                ProcessTime.Value -= Time.deltaTime;
                yield return null;
            }
            
            Provider.OnProvideCombat.Unregister("DrainBuff");
            
            Complete();
        }

        private void Drain(CombatEntity entity)
        {
            Provider.DynamicStatEntry.Hp.Value += entity.Value * drainPercentage;
        }  

        public override void SetUp()
        {
            base.SetUp();
            
            var statusEffectData = MainGame.MainData.StatusEffectSheetData(ActionCode);

            drainPercentage = statusEffectData.ValueList[0];
        }
    }
}
