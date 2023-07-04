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
        private readonly List<ICombatTaker> takerListPerAction = new();
        
        public override void Initialize()
        {
            base.Initialize();
            
            cost.PayCondition.Add("HasTarget", detector.HasTarget);

            Builder
                .AddActiveParam("DashMovement", Dashing)
                .Add(SectionType.Active, "CheckColliding", () => Colliding().Forget())
                .Add(SectionType.Active, "CreatePhantom", CreatePhantom)
                .Add(SectionType.Cancel, "CancelTween", () => Cb.Pathfinding.Cancel())
                .Add(SectionType.End, "StopTask", StopTask);
        }


        private void Dashing(Vector3 targetPosition)
        {
            var playerPosition = Cb.transform.position;
            var direction = (targetPosition - playerPosition).normalized;

            Cb.Pathfinding.Dash(direction, Distance, 0.28f, Invoker.Complete);
        }

        private void CreatePhantom()
        {
            executor.ToPosition(Cb.transform.position, ExecuteGroup.Group2);
        }

        private async UniTaskVoid Colliding()
        {
            cts = new CancellationTokenSource();

            while (Invoker.IsActive)
            {
                var collidedTarget = detector.GetTakersInCircleRange(Range, Angle);

                if (collidedTarget.HasElement())
                {
                    collidedTarget.ForEach(target =>
                    {
                        if (!target.Alive.Value || takerListPerAction.Contains(target)) return;
                        
                        takerListPerAction.Add(target);
                        executor.ToTaker(target);
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
