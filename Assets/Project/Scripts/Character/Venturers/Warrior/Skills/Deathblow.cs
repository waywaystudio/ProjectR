using System.Threading;
using Common.Projectors;
using Common.Skills;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace Character.Venturers.Warrior.Skills
{
    public class Deathblow : SkillComponent
    {
        [FormerlySerializedAs("projector")] [SerializeField] private Projection projection;

        private CancellationTokenSource cts;
        private CancellationTokenSource chargingCts;
        public float ExecuteProgression { get; private set; }
        
        public override void Initialize()
        {
            base.Initialize();
            
            projection.Initialize(this);
            Builder
                .Add(Section.Active, "TargetTracking", () => PlayTracking().Forget())
                .Add(Section.Active, "Charging", () => PlayChargingProgress().Forget())
                .Add(Section.Release, "AimShotRelease", DeathblowRelease)
                .Add(Section.Execute, "SetInvokerIsActiveTrue", () => Invoker.IsActive = false)
                .Add(Section.Execute, "DeathblowExecute", ExecuteDeathblow)
                .Add(Section.End, "StopTracking", Stop)
                .Remove(Section.Release, "ReleaseAction")
                ;
        }
        
        protected override void Dispose()
        {
            base.Dispose();

            Stop();
        }

        
        private void DeathblowRelease()
        {
            if (!Invoker.IsActive) return;

            StopProgression();

            if (ExecuteProgression / CastingTime <= 0.5f)
            {
                Invoker.Cancel();
            }
            else
            {
                Invoker.Execute();
            }
        }

        private void ExecuteDeathblow()
        {
            if (!detector.TryGetTakers(out var takers)) return;
            
            SkillCost.PayCost(Cb.Resource);
            
            takers.ForEach(taker =>
            {
                Taker = taker;
                Invoker.Hit(taker);
            });
        }
        
        private async UniTaskVoid PlayChargingProgress()
        {
            chargingCts        = new CancellationTokenSource();
            ExecuteProgression = 0f;
            
            while (ExecuteProgression < CastTimer.Duration)
            {
                ExecuteProgression += Time.deltaTime;

                await UniTask.Yield(chargingCts.Token);
            }
        }

        private async UniTaskVoid PlayTracking()
        {
            if (Cb is not VenturerBehaviour venturer) return;
            
            cts = new CancellationTokenSource();
            
            if (venturer.IsPlayer)
            {
                while (true)
                {
                    if (!InputManager.TryGetMousePosition(out var mousePosition)) mousePosition = Vector3.zero;
                
                    venturer.Rotate(mousePosition);
                    await UniTask.Yield(cts.Token);
                }
            }

            while (true)
            {
                var mainTarget = detector.GetMainTarget();
                var takerPosition = mainTarget is not null
                    ? mainTarget.Position
                    : Cb.transform.forward * AreaRange;

                venturer.Rotate(takerPosition);
                await UniTask.Yield(cts.Token);
            }
        }
        
        private void Stop()
        {
            StopProgression();
            StopTracking();
        }
        
        private void StopProgression()
        {
            chargingCts?.Cancel();
            chargingCts = null;
        }

        private void StopTracking()
        {
            cts?.Cancel();
            cts = null;
        }
    }
}
