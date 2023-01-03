using System.Collections;
using UnityEngine;

namespace Common.Character.Operation.Combat.StatusEffect.DeBuff
{
    public sealed class CorruptionDeBuff : BaseStatusEffect, ICombatProvider
    {
        private const int TickCount = 5;

        public string Name => Sender.Name;
        public GameObject Object => Sender.Object;
        public Status Status => Sender.Status;
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
        
        public void ReportActive(CombatLog log) => Sender.ReportActive(log);
    }
}
