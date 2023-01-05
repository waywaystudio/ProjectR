using System.Collections;
using UnityEngine;

namespace Character.Combat.StatusEffects.DeBuff
{
    public sealed class CorruptionDeBuff : BaseStatusEffect, ICombatEntity
    {
        private const int TickCount = 5;

        public Status Status => Provider.Status;
        public StatTable StatTable => Provider.StatTable;

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
    }
}
