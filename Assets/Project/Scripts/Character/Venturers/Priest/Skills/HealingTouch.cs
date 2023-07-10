using Common;
using Common.Execution.Hits;
using Common.Skills;
using UnityEngine;

namespace Character.Venturers.Priest.Skills
{
    public class HealingTouch : SkillComponent
    {
        [SerializeField] private HealHit healExecution;

        private Vector3 predicatePosition = Vector3.zero;

        public HealHit HealExecution => healExecution;
        
        public override void Initialize()
        {
            base.Initialize();
            
            cost.PayCondition.Add("HasTarget", HasTarget);

            Builder
                .AddApplying("SavePredicatePosition", TargetHealing)
                .Add(Section.Execute, "ExecuteHealingTouch",ExecuteHealingTouch)
                .Add(Section.Execute, "TryConsumeLightWeaver", TryConsumeLightWeaver);
        }


        private void ExecuteHealingTouch()
        {
            var validTakerList = detector.GetTakersInCircleRange(predicatePosition, 6f, 360f);
            
            validTakerList?.ForEach(taker =>
            {
                Invoker.Hit(taker);
            });
        }
        
        private void TryConsumeLightWeaver()
        {
            Cb.DispelStatusEffect(DataIndex.LightWeaverStatusEffect);
        }
        
        private bool HasTarget()
        {
            // TODO. ExecuteHealingTouch 함수와 중복
            var takers = detector.GetTakersInCircleRange(predicatePosition, 6f, 360f);

            return !takers.IsNullOrEmpty() 
                   && takers[0].Alive.Value;
        }

        private void TargetHealing(Vector3 targetPosition)
        {
            predicatePosition = TargetUtility.GetValidPosition(Cb.Position, Range, targetPosition);
        }
    }
}
