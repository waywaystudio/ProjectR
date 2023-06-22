using Common;
using Common.StatusEffects;
using UnityEngine;

namespace Character.Venturers.Knight.StatusEffects
{
    public class ArmorCrash : StatusEffect
    {
        [SerializeField] private StatEntity armorReduce = new(StatType.Armor, "ArmorCrashDebBuff", -50);

        public override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);

            SequenceBuilder.Add(SectionType.Active,"StatTableRegister", () => Taker.StatTable.Add(armorReduce))
                           .Add(SectionType.Complete,"Return", () => Taker.StatTable.Remove(armorReduce));
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
    }
}
