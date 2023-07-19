using System.Threading;
using Common;
using Common.Skills;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Character.Venturers.Warrior.Skills
{
    public class RapidCharge : SkillComponent
    {
        private CancellationTokenSource cts;
        private readonly Collider[] buffers = new Collider[32];
        
        public override void Initialize()
        {
            base.Initialize();

            Builder
                .AddApplying("RigidMove", RigidMove)
                .Add(Section.Active, "CheckColliding", () => OnCollided().Forget())
                .Add(Section.Execute, "ExecuteRapidCharge", ExecuteRapidCharge)
                .Add(Section.Execute, "PlayCollideAnimation", PlayCollideAnimation)
                .Add(Section.Execute, "StopPathfinding", Cb.Pathfinding.Stop)
                .Add(Section.Execute, "StopCharging", StopCharging)
                .Add(Section.End, "StopCharging", StopCharging);
        }
        
        protected override void Dispose()
        {
            base.Dispose();

            StopCharging();
        }
        
        
        private void ExecuteRapidCharge()
        {
            if (!detector.TryGetTakers(out var takers)) return;
            
            takers.ForEach(taker =>
            {
                Taker = taker;
                Invoker.Hit(taker);
            });
        }

        private async UniTaskVoid OnCollided()
        {
            cts = new CancellationTokenSource();
            ChargingMovement();

            while (true)
            {
                if (detector.TryGetTakersInCircle(transform.position, 2f, buffers, out var takers))
                {
                    if (takers.FirstOrDefault().Alive.Value)
                    {
                        Invoker.Execute();
                        return;
                    }
                }

                await UniTask.Yield(cts.Token);
            }
        }
        
        private void RigidMove(Vector3 targetPosition)
        {
            var venturer = GetComponentInParent<VenturerBehaviour>();
            
            var playerPosition = Cb.transform.position;
            var direction = (targetPosition - playerPosition).normalized;
            var distance = Vector3.Distance(playerPosition, targetPosition);
            var actualDestination = distance <= PivotRange
                ? targetPosition
                : playerPosition + direction * PivotRange;
            
            var destination = venturer.IsPlayer
                ? actualDestination
                : detector.GetMainTarget() is not null
                    ? detector.GetMainTarget().Position
                    : Vector3.zero;
            
            Cb.Pathfinding.Move(destination, Invoker.Complete);
        }
        
        private void PlayCollideAnimation()
        {
            Cb.Animating.PlayOnce("AttackDuelStab", 1f + Haste.Invoke(), Invoker.Complete);
        }

        private void ChargingMovement()
        {
            Cb.StatTable.Add(new StatEntity(StatType.MoveSpeed, "RapidCharge", 20f));
        }

        private void RemoveCharging()
        {
            Cb.StatTable.Remove(StatType.MoveSpeed, "RapidCharge");
        }

        private void StopCharging()
        {
            RemoveCharging();
            
            cts?.Cancel();
            cts = null;
        }
    }
}
