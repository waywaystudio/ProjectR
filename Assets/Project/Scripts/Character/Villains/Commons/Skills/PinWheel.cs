using Common.Projectors;
using Common.Skills;
using DG.Tweening;
using UnityEngine;
using Projector = Common.Projectors.Projector;

namespace Character.Villains.Commons.Skills
{
    public class PinWheel : SkillComponent
    {
        [SerializeField] private Projector forwardProjector;
        [SerializeField] private Projector backwardProjector;
        [SerializeField] private Projector leftwardProjector;
        [SerializeField] private Projector rightwardProjector;

        private Tween rotateTween;
        
        public override void Initialize()
        {
            base.Initialize();
            
            forwardProjector.Initialize(this);
            backwardProjector.Initialize(this);
            rightwardProjector.Initialize(this);
            leftwardProjector.Initialize(this);
            Builder
                .Add(Section.Execute, "HitShockWave", HitShockWave)
                .Add(Section.Active, "SlowRotate", SlowRotate)
                .Add(Section.End, "StopTween", StopTween)
                ;
        }

        protected override void Dispose()
        {
            base.Dispose();

            StopTween();
        }


        private void HitShockWave()
        {
            if (!detector.TryGetTakers(out var takers)) return;
            
            takers.ForEach(taker =>
            {
                Taker = taker;
                Invoker.Hit(taker);
            });
        }

        private void SlowRotate()
        {
            var backward = -Cb.Forward;

            rotateTween = Cb.transform
                            .DOLookAt(backward, 2f)
                            .SetEase(Ease.InOutCubic)
                            ;
        }

        private void StopTween()
        {
            if (rotateTween == null) return;

            rotateTween.Kill();
            rotateTween = null;
        }
    }
}
