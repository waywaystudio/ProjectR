using System.Threading;
using Common;
using Common.Characters;
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

            Builder
                .AddApplying("TargetTracking", targetPosition => ChannelingExtraction(targetPosition).Forget())
                .Add(Section.Execute, "SetIsActiveTrue", () => Invoker.IsActive = false)
                .Add(Section.End, "StopTracking", StopTracking);
        }


        private async UniTaskVoid ChannelingExtraction(Vector3 targetPosition)
        {
            if (Cb is not VenturerBehaviour venturer) return;
            
            cts = new CancellationTokenSource();

            var validPosition = TargetUtility.GetValidPosition(Cb.transform.position, Distance, targetPosition);
                
            venturer.Rotate(validPosition);
            Invoker.SubFire(validPosition);
                
            while (true)
            {
                var validTakerList = detector.GetTakersInCircleRange(validPosition, Range, 360f);
                validTakerList?.ForEach(taker =>
                {
                    Invoker.Hit(taker);
                    Invoker.Fire(taker.Position);
                });
                    
                await UniTask.Delay(500, DelayType.DeltaTime, PlayerLoopTiming.Update, cts.Token);
            }
        }

        private void StopTracking()
        {
            cts?.Cancel();
            cts = null;
        }
    }
}
