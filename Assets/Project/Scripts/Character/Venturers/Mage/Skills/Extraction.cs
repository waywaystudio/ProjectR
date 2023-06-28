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

            SequenceBuilder.AddActiveParam("TargetTracking", targetPosition => ChannelingExtraction(targetPosition).Forget())
                           .Add(SectionType.Execute, "PlayEndChargingAnimation", PlayEndChargingAnimation)
                           .Add(SectionType.Execute, "SetIsActiveTrue", () => SkillInvoker.IsActive = false)
                           .Add(SectionType.End, "StopTracking", StopTracking);
        }


        private void PlayEndChargingAnimation()
        {
            Cb.Animating.PlayOnce("ExtractHoldFire", 1f + Haste, SkillInvoker.Complete);
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
                validTakerList?.ForEach(taker => executor.Execute(taker));
                    
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
