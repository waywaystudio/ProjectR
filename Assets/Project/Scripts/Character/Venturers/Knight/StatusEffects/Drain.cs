using Common;
using Common.StatusEffects;
using UnityEngine;

namespace Character.Venturers.Knight.StatusEffects
{
    public class Drain : StatusEffect
    {
        [SerializeField] private float drainPercentage;

        public override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);

            SequenceBuilder.Add(SectionType.Active,"DrainBuff", () => Provider.OnDamageProvided.Add("DrainBuff", DrainHp))
                           .Add(SectionType.End,"Return", () => Provider.OnDamageProvided.Remove("DrainBuff"));
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
                SequenceInvoker.Complete();
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
