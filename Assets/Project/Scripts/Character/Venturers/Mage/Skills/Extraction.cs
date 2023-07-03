using System.Threading;
using Common.Skills;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Character.Venturers.Mage.Skills
{
    public class Extraction : SkillComponent
    {
        private CancellationTokenSource cts;
        
        public override void Initialize()
        {
            base.Initialize();

            Builder.AddActiveParam("TargetTracking", targetPosition => ChannelingExtraction(targetPosition).Forget())
                           .Add(SectionType.Execute, "PlayEndChargingAnimation", PlayEndChargingAnimation)
                           .Add(SectionType.Execute, "SetIsActiveTrue", () => Invoker.IsActive = false)
                           .Add(SectionType.End, "StopTracking", StopTracking);
        }


        private void PlayEndChargingAnimation()
        {
            Cb.Animating.PlayOnce("ExtractHoldFire", 1f + Haste, Invoker.Complete);
        }
        
        private async UniTaskVoid ChannelingExtraction(Vector3 targetPosition)
        {
            if (Cb is not VenturerBehaviour venturer) return;
            
            cts = new CancellationTokenSource();
            
            var validPosition = ValidPosition(targetPosition);
                
            venturer.Rotate(validPosition);
                
            while (true)
            {
                var validTakerList = detector.GetTakersInCircleRange(validPosition, 6f, 360f);
                validTakerList?.ForEach(taker =>
                {
                    executor.ToTaker(taker);
                    executor.ToPosition(taker.Position);
                });
                    
                await UniTask.Delay(500, DelayType.DeltaTime, PlayerLoopTiming.Update, cts.Token);
            }
        }

        private void StopTracking() => cts?.Cancel();

        private Vector3 ValidPosition(Vector3 targetPosition)
        {
            var playerPosition = Cb.transform.position;
            
            if (Vector3.Distance(playerPosition, targetPosition) <= Range)
            {
                return targetPosition;
            }

            var direction = (targetPosition - playerPosition).normalized;
            var destination = playerPosition + direction * Range;

            return destination;
        }
    }
}
