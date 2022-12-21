using System.Collections;
using Core;
using MainGame.Manager.Combat;
using UnityEngine;

namespace Common.Character.Operation.StatusEffect.DeBuff
{
    public sealed class CorruptionDeBuff : BaseStatusEffect, ICombatProvider
    {
        private const int TickCount = 5;
        private CharacterBehaviour Cb => Object.GetComponent<CharacterBehaviour>();

        public GameObject Object => ProviderInfo.Object;
        public string ProviderName => ProviderInfo.ProviderName;
        public float CombatPower => ProviderInfo.CombatPower * BaseData.CombatValue;
        public float Critical => ProviderInfo.Critical;
        public float Haste => ProviderInfo.Haste;
        public float Hit => ProviderInfo.Hit;

        public override IEnumerator MainAction()
        {
            var corruptionDuration = Duration * CombatManager.GetHasteValue(ProviderInfo.Haste);
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
        
        public void CombatReport(ILog log)
        {
            if (log is CombatLog combatLog)
            {
                Cb.ReportDamage(combatLog);
            } 
        }
    }
}
