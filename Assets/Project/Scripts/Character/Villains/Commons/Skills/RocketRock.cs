using System.Threading;
using Common.Projectors;
using Common.Skills;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Character.Villains.Commons.Skills
{
    public class RocketRock : SkillComponent
    {
        [SerializeField] private RectProjector projector;
        
        private CancellationTokenSource cts;
        
        public override void Initialize()
        {
            base.Initialize();
            
            projector.Initialize(this);
            Builder
                .Add(Section.Active, "PlayTracking", () => PlayTracking().Forget())
                .Add(Section.Execute, "HitRocketRock", HitRocketRock)
                .Add(Section.End, "StopTracking", StopTracking)
                ;
        }

        protected override void Dispose()
        {
            base.Dispose();

            StopTracking();
        }
        

        private void HitRocketRock()
        {
            if (!detector.TryGetTakers(out var takers)) return;
            
            takers.ForEach(taker =>
            {
                Taker = taker;
                Invoker.Hit(taker);
            });
        }
        
        private async UniTaskVoid PlayTracking()
        {
            cts = new CancellationTokenSource();
            
            var mainTarget = detector.GetMainTarget();

            while (Invoker.IsActive)
            {
                var takerPosition = mainTarget is not null
                    ? mainTarget.Position
                    : Cb.transform.forward * AreaRange;

                Cb.Rotate(takerPosition);
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
