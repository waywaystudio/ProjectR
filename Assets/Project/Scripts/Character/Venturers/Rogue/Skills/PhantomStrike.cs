using System.Collections.Generic;
using System.Threading;
using Common;
using Common.Skills;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Character.Venturers.Rogue.Skills
{
    public class PhantomStrike : SkillComponent
    {
        private CancellationTokenSource cts;
        private readonly Collider[] buffers = new Collider[32];
        private readonly List<ICombatTaker> takerListPerAction = new();
        
        public override void Initialize()
        {
            base.Initialize();

            Builder
                .AddApplying("DashMovement", Dashing)
                .Add(Section.Active, "CheckColliding", () => Colliding().Forget())
                .Add(Section.Active, "CreatePhantom", CreatePhantom)
                .Add(Section.Cancel, "CancelTween", () => Cb.Pathfinding.Cancel())
                .Add(Section.End, "StopTask", StopTask);
        }
        
        protected override void Dispose()
        {
            base.Dispose();

            StopTask();
        }


        private void Dashing(Vector3 targetPosition)
        {
            var playerPosition = Cb.transform.position;
            var direction = (targetPosition - playerPosition).normalized;

            Cb.Pathfinding.Dash(direction, PivotRange, 0.28f, Invoker.Complete);
        }

        private void CreatePhantom()
        {
            Invoker.SubFire(Cb.transform.position);
        }

        private async UniTaskVoid Colliding()
        {
            cts = new CancellationTokenSource();

            while (Invoker.IsActive)
            {
                if (detector.TryGetTakersInCircle(transform.position, AreaRange, buffers, out var takers))
                {
                    takers.ForEach(taker =>
                    {
                        if (!taker.Alive.Value || takerListPerAction.Contains(taker)) return;
                        
                        takerListPerAction.Add(taker);
                        Taker = taker;
                        Invoker.Hit(taker);
                    });
                }

                await UniTask.Yield(cts.Token);
            }
        }

        private void StopTask()
        {
            cts?.Cancel();
            cts = null;
            takerListPerAction.Clear();
        }
    }
}
