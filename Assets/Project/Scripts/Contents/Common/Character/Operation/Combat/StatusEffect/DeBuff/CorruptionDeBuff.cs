using System.Collections;
using UnityEngine;

namespace Common.Character.Operation.Combat.StatusEffect.DeBuff
{
    public sealed class CorruptionDeBuff : BaseStatusEffect, ICombatProvider
    {
        private const int TickCount = 5;

        public ICombatProvider Sender => ProviderInfo.Sender;
        public string Name => ProviderInfo.Name;
        public GameObject Object => ProviderInfo.Object;
        public StatTable StatTable => Sender.StatTable;

        public override IEnumerator MainAction()
        {
            var corruptionDuration = Duration * CharacterUtility.GetHasteValue(StatTable.Haste);
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
        
        public void CombatReport(CombatLog log) => Sender.CombatReport(log);
    }
}
