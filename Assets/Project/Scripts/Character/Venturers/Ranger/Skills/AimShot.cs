using System.Threading;
using Common.Skills;
using Cysharp.Threading.Tasks;
using Manager;
using UnityEngine;

namespace Character.Venturers.Ranger.Skills
{
    public class AimShot : SkillComponent
    {
        private CancellationTokenSource cts;
        
        public override void Initialize()
        {
            base.Initialize();

            SequenceBuilder.Add(SectionType.Active, "Tracking", () => PlayTracking().Forget())
                           .Add(SectionType.Execute, "PlayEndChargingAnimation", PlayEndChargingAnimation)
                           .Add(SectionType.Execute, "AimShotExecute", () => executor.Execute(null))
                           .Add(SectionType.Execute, "InsureActiveCoolTime", () => CoolTimer.Play(CoolWeightTime))
                           .Add(SectionType.End, "StopTracking", StopTracking);
        }
        
        public override void Dispose()
        {
            base.Dispose();

            StopTracking();
        }
        

        private void PlayEndChargingAnimation()
        {
            Cb.Animating.PlayOnce("AimHoldFire", 0f, SkillInvoker.Complete);
        }

        private async UniTaskVoid PlayTracking()
        {
            if (Cb is not VenturerBehaviour venturer) return;
            
            cts = new CancellationTokenSource();
            
            if (venturer.IsPlayer)
            {
                while (true)
                {
                    if (!MainManager.Input.TryGetMousePosition(out var mousePosition)) mousePosition = Vector3.zero;
                
                    venturer.Rotate(mousePosition);
                    await UniTask.Delay(100, DelayType.DeltaTime, PlayerLoopTiming.Update, cts.Token);
                }
            }

            while (true)
            {
                var mainTarget = detector.GetMainTarget();
                var takerPosition = mainTarget is not null
                    ? mainTarget.Position
                    : Cb.transform.forward * Range;

                venturer.Rotate(takerPosition);
                await UniTask.Delay(100, DelayType.DeltaTime, PlayerLoopTiming.Update, cts.Token);
            }
        }

        private void StopTracking() => cts?.Cancel();
    }
}
