using System.Threading;
using Common.Skills;
using Cysharp.Threading.Tasks;
using Manager;
using UnityEngine;

namespace Character.Venturers.Ranger.Skills
{
    public class ContinuesAttack : SkillComponent
    {
        private CancellationTokenSource cts;
        
        public override void Initialize()
        {
            base.Initialize();

            SequenceBuilder.Add(SectionType.Execute, "ShotAttackExecution", () => executor.Execute(null))
                           .Add(SectionType.Active, "Tracking", () => PlayTracking().Forget())
                           .Add(SectionType.End, "StopTracking", StopTracking);

        }
        
        public override void Dispose()
        {
            base.Dispose();

            StopTracking();
        }


        private async UniTaskVoid PlayTracking()
        {
            if (Cb is not VenturerBehaviour venturer) return;
            
            cts = new CancellationTokenSource();
            
            if (venturer!.IsPlayer)
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
                if (detector.GetMainTarget() == null)
                {
                    StopTracking();
                }

                var takerPosition = detector.GetMainTarget().Position;  

                venturer.Rotate(takerPosition);
                await UniTask.Delay(100, DelayType.DeltaTime, PlayerLoopTiming.Update, cts.Token);
            }
        }

        private void StopTracking() => cts?.Cancel();
    }
}
