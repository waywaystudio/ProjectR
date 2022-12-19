using System;
using System.Collections;
using Core;
using MainGame.Manager.Combat;
using UnityEngine;

namespace Common.Character.Operation.Combating.StatusEffects
{
    [Serializable]
    public class Corruption : StatusEffect, ICombatProvider
    {
        public Corruption(ICombatTaker taker, ICombatProvider combatInfo) : base("Corruption")
        {
            this.taker = taker;
            this.combatInfo = combatInfo;

            Provider = combatInfo.Provider;
            ProviderName = combatInfo.ProviderName;
            CombatPower = combatInfo.CombatPower * StatusEffectData.CombatValue;
            Critical = combatInfo.Critical;
            Haste = combatInfo.Haste;
            Hit = combatInfo.Hit;
        }
        
        private int tickCount = 5;
        private ICombatTaker taker;
        private ICombatProvider combatInfo;

        public GameObject Provider { get; set; }
        public string ProviderName { get; set; }
        public float CombatPower { get; set; }
        public float Critical { get; set; }
        public float Haste { get; set; }
        public float Hit { get; set; }
        

        public override IEnumerator Invoke()
        {
            var corruptionDuration = Duration * CombatManager.GetHasteValue(combatInfo.Haste);
            var tickInterval = corruptionDuration / tickCount;
            var currentTick = tickInterval;

            for (var i = 0; i < tickCount; ++i)
            {
                while (currentTick > 0)
                {
                    currentTick -= Time.deltaTime;
                    yield return null;
                }
                
                taker.TakeDamage(combatInfo);
                currentTick = tickInterval;
            }

            yield return null;
        }
    }
}
