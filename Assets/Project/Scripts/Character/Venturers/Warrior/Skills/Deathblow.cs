using System.Threading;
using Common.Projectors;
using Common.Skills;
using Cysharp.Threading.Tasks;
using Manager;
using UnityEngine;

namespace Character.Venturers.Warrior.Skills
{
    public class Deathblow : SkillComponent
    {
        [SerializeField] private ArcProjector projector;

        private CancellationTokenSource cts;
        
        public override void Initialize()
        {
            base.Initialize();
            
            projector.Initialize(this);
            cost.PayCondition.Add("HasTarget", detector.HasTarget);

            Builder
                .Add(Section.Active, "TargetTracking", () => PlayTracking().Forget())
                .Add(Section.Execute, "SetInvokerIsActiveTrue", () => Invoker.IsActive = false)
                .Add(Section.Execute, "DeathblowExecute", ExecuteDeathblow)
                .Add(Section.End, "StopTracking", StopTracking);
        }
        
        protected override void Dispose()
        {
            base.Dispose();

            StopTracking();
        }


        private void ExecuteDeathblow()
        {
            detector.GetTakers()?.ForEach(taker =>
            {
                Taker = taker;
                Invoker.Hit(taker);
            });
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
                    await UniTask.Yield(cts.Token);
                }
            }

            while (true)
            {
                var mainTarget = detector.GetMainTarget();
                var takerPosition = mainTarget is not null
                    ? mainTarget.Position
                    : Cb.transform.forward * Range;

                venturer.Rotate(takerPosition);
                await UniTask.Yield(cts.Token);
            }
        }

        private void StopTracking()
        {
            cts?.Cancel();
            cts = null;
        }
    }
}
