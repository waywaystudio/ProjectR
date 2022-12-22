using System.Collections;
using UnityEngine;

namespace Common.Character.Operation.StatusEffect.DeBuff
{
    public sealed class CorruptionDeBuff : BaseStatusEffect, ICombatProvider
    {
        private const int TickCount = 5;

        public ICombatProvider Predecessor => ProviderInfo.Predecessor;
        public string Name => ProviderInfo.Name;
        public GameObject Object => ProviderInfo.Object;
        public CombatValueEntity CombatValue
        {
            get
            {
                var corruptionValue = ProviderInfo.CombatValue;
                corruptionValue.Power = ProviderInfo.CombatValue.Power * BaseData.CombatValue;

                return corruptionValue;
            }
        }

        public override IEnumerator MainAction()
        {
            var corruptionDuration = Duration * CharacterUtility.GetHasteValue(CombatValue.Haste);
            var tickInterval = corruptionDuration / TickCount;
            var currentTick = tickInterval;

            for (var i = 0; i < TickCount; ++i)
            {
                while (currentTick > 0)
                {
                    currentTick -= Time.deltaTime;
                    yield return null;
                }

                TakerInfo.TakeDamage(this);
                currentTick = tickInterval;
            }

            Callback?.Invoke();
        }
        
        public void CombatReport(CombatLog log) => Predecessor.CombatReport(log);
    }
}
