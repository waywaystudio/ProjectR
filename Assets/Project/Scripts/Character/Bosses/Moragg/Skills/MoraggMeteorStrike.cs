using System.Collections;
using System.Collections.Generic;
using Common;
using Common.Skills;
using UnityEngine;

namespace Character.Bosses.Moragg.Skills
{
    public class MoraggMeteorStrike : SkillComponent
    {
        [SerializeField] private int radius = 20;
        [SerializeField] private int minDistance = 10;
        [SerializeField] private int maxPoints = 10;
        [SerializeField] private int sampleSize = 30;
        
        private Coroutine meteorRoutine;
        
        public override void Execution()
        {
            meteorRoutine = StartCoroutine(StartMeteor());
        }
        

        protected override void Initialize()
        {
            OnActivated.Register("Execution", Execution);
            OnCanceled.Register("StopExecution", StopMeteor);
            
            OnCompleted.Register("EndCallback", End);
            OnCompleted.Register("StopExecution", StopMeteor);
        }
        
        protected override void PlayAnimation()
        {
            Cb.Animating.PlayLoop(animationKey);
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
                ExecutionTable.Execute(destination);

                yield return new WaitForSeconds(0.2f);
            }
        }

        private void StopMeteor()
        {
            StopCoroutine(meteorRoutine);
        }

        private List<Vector2> GetPointsInCircle()
            => RandomSampler.GetPointsInCircle(radius, minDistance, maxPoints, sampleSize);
    }
}
