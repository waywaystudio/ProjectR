using System.Collections.Generic;
using System.Threading;
using Common;
using Common.Skills;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Character.Villains.Commons.Skills
{
    public class MeteorStrike : SkillComponent
    {
        [SerializeField] private VillainPhaseMask enableMask;
        [SerializeField] private int radius = 20;
        [SerializeField] private int minDistance = 10;
        [SerializeField] private int maxPoints = 10;
        [SerializeField] private int sampleSize = 30;

        private CancellationTokenSource cts;
        private List<Vector3> DestinationBuffer { get; } = new();


        public override void Initialize()
        {
            base.Initialize();
            
            var villain = GetComponentInParent<VillainBehaviour>();

            Builder
                .AddCondition("ConditionSelfHpStatus", () => (enableMask | villain.CurrentPhase.PhaseMask) == enableMask)
                .Add(Section.Active, "FireMeteor", () => FireMeteor().Forget())
                .Add(Section.End, "StopMeteor", StopMeteor);
        }

        protected override void Dispose()
        {
            base.Dispose();

            StopMeteor();
        }


        private async UniTaskVoid FireMeteor()
        {
            cts = new CancellationTokenSource();
            
            DestinationBuffer.Clear();
            
            GetPointsInCircle().ForEach(point =>
            {
                DestinationBuffer.Add(new Vector3(point.x, 0f, point.y));
            });

            foreach (var destination in DestinationBuffer)
            {
                Invoker.Fire(destination);

                await UniTask.Delay(200, cancellationToken: cts.Token);
            }
        }

        private void StopMeteor()
        {
            cts?.Cancel();
            cts = null;
        }

        private List<Vector2> GetPointsInCircle()
            => RandomSampler.GetPointsInCircle(radius, minDistance, maxPoints, sampleSize);
    }
}
