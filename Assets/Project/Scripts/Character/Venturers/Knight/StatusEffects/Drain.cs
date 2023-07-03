using System.Threading;
using Common;
using Common.StatusEffects;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Character.Venturers.Knight.StatusEffects
{
    public class Drain : StatusEffect
    {
        [SerializeField] private float drainPercentage;

        private CancellationTokenSource cts;

        public override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);

            Builder
                .Add(SectionType.Active,"DrainBuff", () => Provider.OnDamageProvided.Add("DrainBuff", DrainHp))
                .Add(SectionType.Active, "OvertimeExecution", () => OvertimeExecution().Forget())
                .Add(SectionType.End, "Stop", Stop)
                .Add(SectionType.End,"Return", () => Provider.OnDamageProvided.Remove("DrainBuff"));
        }


        private void DrainHp(CombatEntity entity)
        {
            Provider.Hp.Value += entity.Value * drainPercentage;
        }

        private async UniTaskVoid OvertimeExecution()
        {
            cts = new CancellationTokenSource();
            
            while (ProgressTime.Value > 0)
            {
                ProgressTime.Value -= Time.deltaTime;

                await UniTask.Yield(cts.Token);
            }
            
            Invoker.Complete();
        }
        
        private void Stop()
        {
            cts?.Cancel();
            cts = null;
        }


#if UNITY_EDITOR
        public override void EditorSetUp()
        {
            base.EditorSetUp();
            
            var statusEffectData = Database.StatusEffectSheetData(DataIndex);

            drainPercentage = statusEffectData.ValueList[0];
        }
#endif
    }
}
