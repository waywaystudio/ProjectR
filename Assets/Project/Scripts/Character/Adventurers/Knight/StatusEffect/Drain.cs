using System.Collections;
using Common;
using Common.StatusEffect;
using UnityEngine;

namespace Adventurers.Knight.StatusEffect
{
    public class Drain : StatusEffectComponent
    {
        [SerializeField] private float drainPercentage;

        public override void Initialized(ICombatProvider provider)
        {
            base.Initialized(provider);
            
            OnActivated.Register("DrainBuff",() => Provider.OnDamageProvided.Register("DrainBuff", DrainHp));
            OnCompleted.Register("Return", () => Provider.OnDamageProvided.Unregister("DrainBuff"));
        }

        public override void Disposed()
        {
            OnActivated.Unregister("DrainBuff");
            OnCompleted.Unregister("Return");
            
            Destroy(gameObject);
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
            
            var statusEffectData = Database.StatusEffectSheetData(ActionCode);

            drainPercentage = statusEffectData.ValueList[0];
        }
#endif
    }
}
