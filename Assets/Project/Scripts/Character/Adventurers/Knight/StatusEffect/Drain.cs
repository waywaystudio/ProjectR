using Common;
using Common.StatusEffect;
using UnityEngine;

namespace Character.Adventurers.Knight.StatusEffect
{
    public class Drain : StatusEffectComponent
    {
        [SerializeField] private float drainPercentage;

        public override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);
            
            OnActivated.Register("DrainBuff",() => Provider.OnDamageProvided.Register("DrainBuff", DrainHp));
            OnCanceled.Register("Dispel", () => Provider.OnDamageProvided.Unregister("DrainBuff"));
            OnCompleted.Register("Return", () => Provider.OnDamageProvided.Unregister("DrainBuff"));
        }


        private void DrainHp(CombatEntity entity)
        {
            Provider.DynamicStatEntry.Hp.Value += entity.Value * drainPercentage;
        }
        
        private void Update()
        {
            if (ProgressTime.Value > 0)
            {
                ProgressTime.Value -= Time.deltaTime;
            }
            else
            {
                Complete();
            }
        }


#if UNITY_EDITOR
        public override void EditorSetUp()
        {
            base.EditorSetUp();
            
            var statusEffectData = Database.StatusEffectSheetData(DataIndex);

            drainPercentage = statusEffectData.ValueList[0];
        }
#endif
    }
}
