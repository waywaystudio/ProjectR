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
            
            cost.PayCondition.Add("HasTarget", HasTarget);

            Builder.AddActiveParam("DashMovement", Dashing)
                           .Add(SectionType.Active, "CheckColliding", () => Colliding().Forget())
                           .Add(SectionType.Active, "CreatePhantom", CreatePhantom)
                           .Add(SectionType.End, "StopCheckColliding", StopChecking);
        }


        private void Dashing(Vector3 targetPosition)
        {
            var playerPosition = Cb.transform.position;
            var direction = (targetPosition - playerPosition).normalized;
            
            Cb.Pathfinding.Dash(direction, Range, 0.28f, Invoker.Complete);
        }

        private void CreatePhantom()
        {
            executor.ToPosition(Cb.transform.position, ExecuteGroup.Group2);
        }

        private bool HasTarget()
        {
            var takers = detector.GetTakers();

            return !takers.IsNullOrEmpty() 
                   && takers[0].Alive.Value;
        }

        private async UniTaskVoid Colliding()
        {
            cts = new CancellationTokenSource();

            while (Invoker.IsActive)
            {
                var collidedTarget = detector.GetTakersInCircleRange(3f, 360);

                if (collidedTarget.HasElement() && 
                    collidedTarget[0].Alive.Value &&
                    !takerListPerAction.Contains(collidedTarget[0]))
                {
                    takerListPerAction.Add(collidedTarget[0]);
                    executor.ToTaker(collidedTarget[0]);
                }
                
                await UniTask.Yield(cts.Token);
            }
        }

        private void StopChecking()
        {
            cts?.Cancel();
            takerListPerAction.Clear();
        }
    }
}
