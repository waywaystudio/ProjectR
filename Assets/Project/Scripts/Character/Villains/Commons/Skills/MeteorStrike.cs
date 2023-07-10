using System.Collections;
using System.Collections.Generic;
using Common;
using Common.Execution;
using Common.Skills;
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
        
        private Coroutine meteorRoutine;
        

        public override void Initialize()
        {
            base.Initialize();
            
            var villain = GetComponentInParent<VillainBehaviour>();

            Builder.AddCondition("ConditionSelfHpStatus", () => (enableMask | villain.CurrentPhase.PhaseMask) == enableMask)
                           .Add(Section.Active, "DirectExecuteMeteorStrike", Invoker.Execute)
                           .Add(Section.Execute, "StartMeteor", () => meteorRoutine = StartCoroutine(StartMeteor()))
                           .Add(Section.End, "StopExecution", StopMeteor);
        }


        private IEnumerator StartMeteor()
        {
            var destinationList = new List<Vector3>();
            
            GetPointsInCircle().ForEach(point =>
            {
                destinationList.Add(new Vector3(point.x, 0f, point.y));
            });

            foreach (var destination in destinationList)
            {
                Invoker.Fire(destination);

                yield return new WaitForSeconds(0.2f);
            }
        }

        private void StopMeteor()
        {
            if (meteorRoutine is null) return;
            
            StopCoroutine(meteorRoutine);
        }

        private List<Vector2> GetPointsInCircle()
            => RandomSampler.GetPointsInCircle(radius, minDistance, maxPoints, sampleSize);
    }
}
