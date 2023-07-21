using System.Threading;
using Common;
using Common.Projectors;
using Common.Skills;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace Character.Venturers.Ranger.Skills
{
    public class AimShot : SkillComponent
    {
        [FormerlySerializedAs("projector")] [SerializeField] private Projection projection;
        
        private CancellationTokenSource cts;
        private CancellationTokenSource chargingCts;
        public float ExecuteProgression { get; private set; }

        public override void Initialize()
        {
            base.Initialize();

            projection.Initialize(this);
            Provider.OnCombatProvided.Add("AddAdrenalinByAimShot", AddAdrenalin);
            Builder
                .Add(Section.Active, "Tracking", () => PlayTracking().Forget())
                .Add(Section.Active, "Charging", () => PlayChargingProgress().Forget())
                .Add(Section.Release, "AimShotRelease", AimShotRelease)
                .Add(Section.Cancel, "StopTracking", Stop)
                .Add(Section.Execute, "Invoker.ActiveTrue", () => Invoker.IsActive = false)
                .Add(Section.Execute, "Fire", Fire)
                .Add(Section.End, "StopTracking", Stop)
                .Remove(Section.Release, "ReleaseAction");
        }
        
        protected override void Dispose()
        {
            base.Dispose();

            Stop();
        }
        
        
        private void Fire()
        {
            var forwardPosition = Provider.Position + Provider.Forward;

            Invoker.Fire(forwardPosition);
        }

        private void AddAdrenalin(CombatEntity damageLog)
        {
            if (damageLog.CombatIndex != DataIndex) return;
            if (damageLog.IsCritical)
            {
                Invoker.SubHit(Cb);
            }
            else
            {
                if (!Cb.StatusEffectTable
                       .TryGetValue(DataIndex.AdrenalinStatusEffect, out var statusEffect)) return;
                
                statusEffect.Dispel();
            }
        }

        private void AimShotRelease()
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
                await UniTask.Delay(100, DelayType.DeltaTime, PlayerLoopTiming.Update, cts.Token);
            }
        }

        private async UniTaskVoid PlayChargingProgress()
        {
            chargingCts = new CancellationTokenSource();
            ExecuteProgression    = 0f;
            
            while (ExecuteProgression < CastTimer.Duration)
            {
                ExecuteProgression += Time.deltaTime;

                await UniTask.Yield(chargingCts.Token);
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
