using System;
using System.Threading;
using Common;
using Common.StatusEffects;
using Common.TargetSystem;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Character.Villains.Commons.StatusEffects
{
    public class IgnitionStatusEffect : StatusEffect, IProjectorSequencer
    {
        [SerializeField] private float interval;
        [SerializeField] private float radius = 6f;
        [SerializeField] private LayerMask adventurerLayer;

        private readonly Collider[] buffers = new Collider[32];
        private CancellationTokenSource cts;
        private float hasteWeight;
        private float tickBuffer;

        
        public float CastWeightTime => Duration;
        public Vector2 SizeVector => new (radius * 2f, radius * 2f);
        

        public override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);

            Builder
                .Add(SectionType.Active, "SetHasteWeight", SetHasteWeight)
                .Add(SectionType.Active, "OvertimeExecution", () => OvertimeExecution().Forget())
                .Add(SectionType.Complete, "Bomb", Bomb)
                .Add(SectionType.End, "Stop", Stop);
        }


        private void Bomb()
        {
            Array.Clear(buffers, 0, buffers.Length); 
            
            var takerList =
                TargetUtility.GetTargetsInSphere<ICombatTaker>(Taker.gameObject.transform.position, adventurerLayer, radius, buffers);

            takerList?.ForEach(taker => executor.ToTaker(taker, ExecuteGroup.Group2));
        }
        
        private void SetHasteWeight() => hasteWeight = tickBuffer = 
            interval * CombatFormula.GetHasteValue(Provider.StatTable.Haste);

        private async UniTaskVoid OvertimeExecution()
        {
            cts = new CancellationTokenSource();

            while (ProgressTime.Value > 0)
            {
                if (tickBuffer > 0f)
                {
                    ProgressTime.Value -= Time.deltaTime;
                    tickBuffer         -= Time.deltaTime;
                }
                else
                {
                    executor.ToTaker(Taker, ExecuteGroup.Group1);
                    tickBuffer = hasteWeight;
                }

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
