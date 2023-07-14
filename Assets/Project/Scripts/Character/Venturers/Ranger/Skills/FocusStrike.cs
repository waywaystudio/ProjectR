using System.Threading;
using Common;
using Common.Skills;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Character.Venturers.Ranger.Skills
{
    public class FocusStrike : SkillComponent
    {
        private CancellationTokenSource cts;
        
        public override void Initialize()
        {
            base.Initialize();

            Provider.OnCombatProvided.Add("AddAdrenalinByAimShot", AddAdrenalin);
            Builder
                .Add(Section.Active, "Tracking", () => PlayTracking().Forget())                           
                .Add(Section.Execute, "TryConsumeEcstasy", TryConsumeEcstasy)
                .Add(Section.Execute, "Fire", Fire)
                .Add(Section.End, "StopTracking", StopTracking);
        }
        
        
        protected override void Dispose()
        {
            base.Dispose();

            StopTracking();
        }

        private void Fire()
        {
            var forwardPosition = Provider.Position + Provider.Forward;

            Invoker.Fire(forwardPosition);
        }

        private void TryConsumeEcstasy()
        {
            Cb.DispelStatusEffect(DataIndex.HuntersEcstasyStatusEffect);
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

        private void StopTracking()
        {
            cts?.Cancel();
            cts = null;
        }
    }
}
