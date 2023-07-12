using System.Threading;
using Common;
using Common.StatusEffects;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Character.Venturers.Knight.StatusEffects
{
    public class ArmorCrash : StatusEffect
    {
        [SerializeField] private float reduceAmount = 50;

        private CancellationTokenSource cts;
        

        public override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);

            Builder
                .Add(Section.Active,"ReduceArmorStat", ReduceArmorStat)
                .Add(Section.Active, "OvertimeExecution", () => OvertimeExecution().Forget())
                .Add(Section.End,"RemoveArmorReducer", RemoveArmorReducer);
        }
        
        protected override void Dispose()
        {
            base.Dispose();

            Stop();
        }


        private void ReduceArmorStat()
        {
            var reduceEntity = new StatEntity(StatType.Armor, "ArmorCrashDebBuff", -reduceAmount);

            Taker.StatTable.Add(reduceEntity);
        }

        private void RemoveArmorReducer()
        {
            Taker.StatTable.Remove(StatType.Armor, "ArmorCrashDebBuff");

            Stop();
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
    }
}
