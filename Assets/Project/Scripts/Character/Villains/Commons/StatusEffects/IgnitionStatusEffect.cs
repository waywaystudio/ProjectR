using System.Threading;
using Common;
using Common.Projectors;
using Common.StatusEffects;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Projector = Common.Projectors.Projector;

namespace Character.Villains.Commons.StatusEffects
{
    public class IgnitionStatusEffect : StatusEffect, IProjection
    {
        [SerializeField] private Projector projector;
        [SerializeField] private float interval;
        [SerializeField] private float radius = 6f;
        [SerializeField] private LayerMask adventurerLayer;

        private readonly Collider[] buffers = new Collider[16];
        private CancellationTokenSource cts;
        private float hasteWeight;
        private float tickBuffer;

        public float CastingTime => Duration;
        public SizeEntity SizeEntity => new (radius, radius,360f);
        

        public override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);
            
            projector.Initialize(this);

            Builder
                .Add(Section.Active, "SetHasteWeight", SetHasteWeight)
                .Add(Section.Active, "OvertimeExecution", () => OvertimeExecution().Forget())
                .Add(Section.Complete, "Bomb", Bomb)
                .Add(Section.End, "Stop", Stop);
        }
        
        protected override void Dispose()
        {
            base.Dispose();

            Stop();
        }

        private void Bomb()
        {
            var takerList =
                TargetUtility.GetTargetsInSphere<ICombatTaker>(Taker.gameObject.transform.position, adventurerLayer, radius, buffers);

            takerList?.ForEach(taker => Invoker.SubHit(taker));
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
                    Invoker.Hit(Taker);
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
