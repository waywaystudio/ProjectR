using Common;
using Common.StatusEffect;
using UnityEngine;

namespace Character.Adventurers.Knight.StatusEffect
{
    public class ArmorCrash : StatusEffectComponent
    {
        [SerializeField] private Stat armorReduce = new(StatType.Armor, StatApplyType.PercentDecrease, 5);

        public override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);
            
            OnActivated.Register("StatTableRegister", () => Taker.StatTable.Add("ArmorCrash", armorReduce));
            OnCompleted.Register("Return", () => Taker.StatTable.Remove("ArmorCrash", armorReduce));
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
    }
}
