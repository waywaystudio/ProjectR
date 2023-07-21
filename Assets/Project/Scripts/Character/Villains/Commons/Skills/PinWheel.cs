using Common.Projectors;
using Common.Skills;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace Character.Villains.Commons.Skills
{
    public class PinWheel : SkillComponent
    {
        [FormerlySerializedAs("forwardProjector")] [SerializeField] private Projection forwardProjection;
        [FormerlySerializedAs("backwardProjector")] [SerializeField] private Projection backwardProjection;
        [FormerlySerializedAs("leftwardProjector")] [SerializeField] private Projection leftwardProjection;
        [FormerlySerializedAs("rightwardProjector")] [SerializeField] private Projection rightwardProjection;

        private Tween rotateTween;
        
        public override void Initialize()
        {
            base.Initialize();
            
            forwardProjection.Initialize(this);
            backwardProjection.Initialize(this);
            rightwardProjection.Initialize(this);
            leftwardProjection.Initialize(this);
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
