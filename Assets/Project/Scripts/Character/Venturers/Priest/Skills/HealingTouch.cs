using Common;
using Common.Characters;
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
            
            cost.PayCondition.Add("HasTarget", detector.HasTarget);

            Builder
                .AddApplying("SavePredicatePosition", TargetHealing)
                .Add(Section.Execute, "ExecuteHealingTouch",ExecuteHealingTouch)
                .Add(Section.Execute, "TryConsumeLightWeaver", TryConsumeLightWeaver);
        }


        private void ExecuteHealingTouch()
        {
            var validTakerList = detector.GetTakersInCircleRange(predicatePosition, Range, Angle);
            
            validTakerList?.ForEach(taker =>
            {
                Taker = taker;
                Invoker.Hit(taker);
            });
        }
        
        private void TryConsumeLightWeaver()
        {
            Cb.DispelStatusEffect(DataIndex.LightWeaverStatusEffect);
        }

        private void TargetHealing(Vector3 targetPosition)
        {
            predicatePosition = TargetUtility.GetValidPosition(Cb.Position, Range, targetPosition);
        }
    }
}
