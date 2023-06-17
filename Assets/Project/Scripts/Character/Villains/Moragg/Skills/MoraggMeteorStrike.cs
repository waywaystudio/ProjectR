using System.Collections;
using System.Collections.Generic;
using Common;
using Common.Skills;
using UnityEngine;

namespace Character.Villains.Moragg.Skills
{
    public class MoraggMeteorStrike : SkillComponent
    {
        [SerializeField] private int radius = 20;
        [SerializeField] private int minDistance = 10;
        [SerializeField] private int maxPoints = 10;
        [SerializeField] private int sampleSize = 30;
        
        private Coroutine meteorRoutine;

        public override void Execution() => ExecuteAction.Invoke();
        

        protected override void AddSkillSequencer()
        {
            sequencer.ActiveAction.Add("DirectExecuteMeteorStrike", Execution);
            sequencer.EndAction.Add("StopExecution", StopMeteor);
            
            ExecuteAction.Add("StartMeteor", () => meteorRoutine = StartCoroutine(StartMeteor()));
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
                executor.Execute(destination);

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
