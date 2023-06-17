using Common;
using Common.StatusEffect;
using UnityEngine;

namespace Character.Venturers.Knight.StatusEffect
{
    public class ArmorCrash : StatusEffectComponent
    {
        [SerializeField] private StatEntity armorReduce = new(StatType.Armor, "ArmorCrashDebBuff", -50);

        public override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);

            SequenceBuilder.AddActive("StatTableRegister", () => Taker.StatTable.Add(armorReduce))
                           .AddComplete("Return", () => Taker.StatTable.Remove(armorReduce));
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
