using System.Threading;
using Common;
using Common.Skills;
using Cysharp.Threading.Tasks;
using Manager;
using UnityEngine;

namespace Character.Venturers.Ranger.Skills
{
    public class FocusStrike : SkillComponent
    {
        private CancellationTokenSource cts;
        
        public override void Initialize()
        {
            base.Initialize();

            Provider.OnDamageProvided.Add("AddAdrenalinByAimShot", AddAdrenalin);
            SequenceBuilder.Add(SectionType.Active, "Tracking", () => PlayTracking().Forget())
                           .Add(SectionType.Execute, "PlayCastCompleteAnimation", PlayCastCompleteAnimation)
                           .Add(SectionType.Execute, "TryConsumeEcstasy", TryConsumeEcstasy)
                           .Add(SectionType.Execute, "FocusShotExecute", () => executor.Execute(null))
                           .Add(SectionType.End, "StopTracking", StopTracking);
        }
        
        public override void Dispose()
        {
            base.Dispose();

            Provider.OnDamageProvided.Remove("AddAdrenalinByInstantShot");
            StopTracking();
        }
        
        
        private void PlayCastCompleteAnimation()
        {
            Cb.Animating.PlayOnce("AimHoldFire", 1f + Haste, SkillInvoker.Complete);
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
                executor.Execute(ExecuteGroup.Group2, Cb);
            }
            else
            {
                if (!Cb.DynamicStatEntry.StatusEffectTable
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
