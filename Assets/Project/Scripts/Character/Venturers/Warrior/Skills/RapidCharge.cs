using System.Collections.Generic;
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
        
        public override void Initialize()
        {
            base.Initialize();

            Builder.AddActiveParam("RigidMove", RigidMove)
                           .Add(SectionType.Active, "CheckColliding", () => OnCollided().Forget())
                           .Add(SectionType.Execute, "CommonExecution", () => detector.GetTakers()?.ForEach(executor.ToTaker))
                           .Add(SectionType.Execute, "PlayCollideAnimation", PlayCollideAnimation)
                           .Add(SectionType.Execute, "StopPathfinding", Cb.Pathfinding.Stop)
                           .Add(SectionType.Execute, "StopCharging", StopCharging)
                           .Add(SectionType.End, "StopCharging", StopCharging);
        }
        

        private async UniTaskVoid OnCollided()
        {
            cts = new CancellationTokenSource();
            ChargingMovement();

            while (true)
            {
                var takerListBuffer = detector.GetTakersInCircleRange(2f, 120);
                
                if (!takerListBuffer.IsNullOrEmpty() && takerListBuffer[0].Alive.Value)
                {
                    Invoker.Execute();
                    return;
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
            var actualDestination = distance <= Range
                ? targetPosition
                : playerPosition + direction * Range;
            
            var destination = venturer.IsPlayer
                ? actualDestination
                : detector.GetMainTarget() is not null
                    ? detector.GetMainTarget().Position
                    : Vector3.zero;
            
            Cb.Pathfinding.Move(destination, Invoker.Complete);
        }
        
        private void PlayCollideAnimation()
        {
            Cb.Animating.PlayOnce("AttackDuelStab", 1f + Haste, Invoker.Complete);
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
        }
    }
}
